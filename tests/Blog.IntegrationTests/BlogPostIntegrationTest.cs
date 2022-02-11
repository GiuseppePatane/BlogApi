using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain.Entities;
using Blog.Infrastructure.Db.EF;
using Blog.Infrastructure.Db.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blog.IntegrationTests;

public class BlogPostIntegrationTest : IntegrationTestBase
{
    [Fact]
    public async Task Create_New_BlogPost_WithInvalidAuthor_ShouldThrowsAnException()
    {
        await using var context = new BlogDbContext(GenDbContextOptions());
        await PrepareDatabase(context).ConfigureAwait(false);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("categoryId", "musica");
        var tag = Tag.Create("tag1", "tag");
        await context.AddAsync(category);
        await context.AddAsync(tag);
        await context.SaveChangesAsync();
        var repository = new EfRepository(context);
        await Assert.ThrowsAsync<DbUpdateException>(() => repository.AddAsync(BlogPost.Create("test", "il grande test",
            "sfsdfds", "https://cdn.com/image.jpg", author, category, new List<Tag> { tag })));
    }

    [Fact]
    public async Task Create_New_BlogPost_Should_StoreIt_In_The_Db()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await PrepareDatabase(context).ConfigureAwait(false);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("categoryId", "musica");
        var tag = Tag.Create("tag1", "tag1");
        context.Authors.Add(author);
        context.Categories.Add(category);
        context.Tags.Add(tag);
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var content = @"<!DOCTYPE html>
                        <html>
                        <head>
                        <title>Page Title</title>
                        </head>
                        <body>

                        <h1>My First Heading</h1>
                        <p>My first paragraph.</p>

                        </body>
                        </html>
                        ";
        await repository.AddAsync(BlogPost.Create("test", "il grande test", content, "https://cdn.com/image.jpg",
            author, category, new List<Tag> { tag }));
        await context.SaveChangesAsync();
        //ASSERT 
        var blogPost = await repository.GetByIdAsync<BlogPost>("test");
        blogPost.Should().NotBeNull();
        blogPost.Id.Should().Be("test");
        blogPost.Title.Should().Be("il grande test");
        blogPost.Content.Should().Be(content);
        blogPost.Image.Should().Be("https://cdn.com/image.jpg");
        blogPost.AuthorId.Should().Be(author.Id);
        blogPost.CategoryId.Should().Be(category.Id);
        await CheckDatabaseAndRemoveIt(context);
    }

    [Fact]
    public async Task Modify_Existing_BlogPost_Should_NotThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await PrepareDatabase(context).ConfigureAwait(false);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("categoryId", "musica");
        var tag = Tag.Create("tag1", "tag1");
        context.BlogPosts.Add(BlogPost.Create("test", "il grande test", "test", "https://cdn.com/image.jpg", author,
            category, new List<Tag> { tag }));
        context.Authors.Add(author);
        context.Categories.Add(category);
        context.Tags.Add(tag);
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var blogPost = await repository.GetByIdAsync<BlogPost>("test");
        blogPost.Update("il piccolo test", "test2", "https://cdn.com/image2.jpg");
        await repository.UpdateAsync(blogPost);
        //ASSERT 
        blogPost = await repository.GetByIdAsync<BlogPost>("test");
        blogPost.Should().NotBeNull();
        blogPost.Id.Should().Be("test");
        blogPost.Title.Should().Be("il piccolo test");
        blogPost.Content.Should().Be("test2");
        blogPost.Image.Should().Be("https://cdn.com/image2.jpg");
        blogPost.UpdateDateUtc.Should().BeAfter(DateTime.UtcNow.Date);
        await CheckDatabaseAndRemoveIt(context);
    }

    [Fact]
    public async Task Associate_Post_To_Existing_Tag_Should_NotThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await PrepareDatabase(context).ConfigureAwait(false);
        var author = Author.Create("test", "grande scrittore");
        var tag = Tag.Create("newTag", "il grande tag");
        var tag2 = Tag.Create("newTag2", "il grande tag2");
        var category = Category.Create("categoryId", "musica");
        context.Tags.Add(tag);
        context.Tags.Add(tag2);
        context.BlogPosts.Add(BlogPost.Create("test", "il grande test", "test", "https://cdn.com/image.jpg", author,
            category, new List<Tag> { tag }));
        context.Authors.Add(author);
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var blogPost = await repository.GetByIdAsync<BlogPost>("test");
        blogPost.AssociateTag(tag2);
        await repository.UpdateAsync(blogPost);
        //ASSERT 
        blogPost = context.BlogPosts.Include(x => x.TagXBlogPosts).FirstOrDefault(x => x.Id == "test");
        blogPost.TagXBlogPosts.Count.Should().Be(2);
        blogPost.TagXBlogPosts.Any(x => x.TagId == tag2.Id).Should().BeTrue();
        await CheckDatabaseAndRemoveIt(context);
    }

    [Fact]
    public async Task Associate_Post_To_Existing_Category_Should_NotThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await PrepareDatabase(context).ConfigureAwait(false);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("newCategory", "music");
        var tag = Tag.Create("tag1", "tag1");
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        context.BlogPosts.Add(BlogPost.Create("test", "il grande test", "test", "https://cdn.com/image.jpg", author,
            category, new List<Tag> { tag }));
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var blogPost = await repository.GetByIdAsync<BlogPost>("test");
        blogPost.UpdateCategory(category);
        await repository.UpdateAsync(blogPost);
        blogPost = await repository.GetByIdAsync<BlogPost>("test");
        //VERIFY 
        blogPost.CategoryId.Should().Be(category.Id);
        await CheckDatabaseAndRemoveIt(context);
    }

    [Fact]
    public async Task Modify_NotExisting_BlogPost_Should_ThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await PrepareDatabase(context).ConfigureAwait(false);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("categoryId", "musica");
        var tag = Tag.Create("tag1", "tag1");
        context.Authors.Add(author);
        context.Categories.Add(category);
        context.Tags.Add(tag);
        await context.SaveChangesAsync();
        var repository = new EfRepository(context);
        //VERIFY 
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
            repository.UpdateAsync(BlogPost.Create("test", "il grande test", "test", "https://cdn.com/image.jpg",
                author, category, new List<Tag> { tag })));
        await CheckDatabaseAndRemoveIt(context);
    }

    [Fact]
    public async Task Delete_Existing_BlogPost_Should_NotThrowsAnException()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await PrepareDatabase(context).ConfigureAwait(false);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("categoryId", "musica");
        var tag = Tag.Create("tag1", "tag1");
        context.Authors.Add(author);
        context.Categories.Add(category);
        context.Tags.Add(tag);
        context.BlogPosts.Add(BlogPost.Create("test", "il grande test", "test", "https://cdn.com/image.jpg", author,
            category, new List<Tag> { tag }));
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var blogPost = await repository.GetByIdAsync<BlogPost>("test");
        await repository.DeleteAsync<BlogPost>(blogPost);
        blogPost = await repository.GetByIdAsync<BlogPost>("test");
        //VERIFY 
        blogPost.Should().BeNull();
        await CheckDatabaseAndRemoveIt(context);
    }
}