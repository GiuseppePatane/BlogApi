using System;
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
        var repository = new EfRepository(context);
        await  Assert.ThrowsAsync<DbUpdateException>(()=> repository.AddAsync(BlogPost.Create("test", "il grande test", "sfsdfds","https://cdn.com/image.jpg",author,category)));
    }
    [Fact]
    public async Task Create_New_BlogPost_Should_StoreIt_In_The_Db()
    {
        // SETUP
        await using var context = new BlogDbContext(GenDbContextOptions());
        await PrepareDatabase(context).ConfigureAwait(false);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("categoryId", "musica");
        context.Authors.Add( author);
        context.Categories.Add( category);
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
        await repository.AddAsync(BlogPost.Create("test", "il grande test", content,"https://cdn.com/image.jpg",author,category));
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
        context.BlogPosts.Add(BlogPost.Create("test", "il grande test", "test","https://cdn.com/image.jpg",author,category));
        context.Authors.Add(author);
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var blogPost = await repository.GetByIdAsync<BlogPost>("test");
        blogPost.Update("il piccolo test","test2","https://cdn.com/image2.jpg");
        await repository.UpdateAsync<BlogPost>(blogPost);
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
        var category = Category.Create("categoryId", "musica");
        context.Tags.Add(tag);
        context.BlogPosts.Add(BlogPost.Create("test", "il grande test", "test","https://cdn.com/image.jpg",author,category));
        context.Authors.Add(author);
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var blogPost = await repository.GetByIdAsync<BlogPost>("test");
        blogPost.AssociateWithTag(tag);
        await repository.UpdateAsync<BlogPost>(blogPost);
        //ASSERT 
        blogPost = context.BlogPosts.Include(x => x.TagXBlogPosts).FirstOrDefault(x => x.Id == "test");
        blogPost.TagXBlogPosts.Count.Should().Be(1);
        blogPost.TagXBlogPosts.Any(x => x.TagId == tag.Id).Should().BeTrue();
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
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.BlogPosts.Add(BlogPost.Create("test", "il grande test", "test","https://cdn.com/image.jpg",author,category));
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var blogPost = await repository.GetByIdAsync<BlogPost>("test");
        blogPost.AssociateWithCategory(category);
        await repository.UpdateAsync<BlogPost>(blogPost);
        //ASSERT 
        blogPost = await repository.GetByIdAsync<BlogPost>("test");
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
        context.Authors.Add(author);
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
            repository.UpdateAsync<BlogPost>(BlogPost.Create("test", "il grande test", "test","https://cdn.com/image.jpg",author,category)));

        //ASSERT 
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
        context.Authors.Add(author);
        context.Categories.Add(category);
        context.BlogPosts.Add(BlogPost.Create("test", "il grande test", "test","https://cdn.com/image.jpg",author,category));
        await context.SaveChangesAsync();
        //ATTEMPT 
        var repository = new EfRepository(context);
        var blogPost = await repository.GetByIdAsync<BlogPost>("test");

        await repository.DeleteAsync<BlogPost>(blogPost);
        //ASSERT 
        blogPost = await repository.GetByIdAsync<BlogPost>("test");
        blogPost.Should().BeNull();
        await CheckDatabaseAndRemoveIt(context);
    }
}