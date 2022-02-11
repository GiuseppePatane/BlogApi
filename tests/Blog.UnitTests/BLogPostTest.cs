using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Blog.UnitTests;

public class BLogPostTest
{
    [Fact]
    public void Create_New_BlogPost_WithValidData_Should_NotThrow_Exceptions()
    {
        //SETUP
        var author = Author.Create("testId", "pippo");
        var category = Category.Create("categoryId", "musica");
        var tag = Tag.Create("tag1", "tag1");
        //ATTEMPT
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>", "image.jpg", author, category,
            new List<Tag> { tag });
        //VERIFY
        blogPost.Should().NotBeNull();
        blogPost.Id.Should().Be("id");
        blogPost.Title.Should().Be("il grande post");
        blogPost.Content.Should().Be("<html></html>");
        blogPost.Image.Should().Be("image.jpg");
        blogPost.CategoryId.Should().Be(category.Id);
        blogPost.AuthorId.Should().Be(author.Id);
        blogPost.TagXBlogPosts.Any(x => x.TagId == tag.Id).Should().BeTrue();
        author.BLogPosts.Should().BeEmpty();
    }

    [Fact]
    public void Create_New_BlogPost_WithInValidData_Should_Throw_Exceptions()
    {
        Assert.Throws<DomainException>(() => BlogPost.Create("id", "il grande post", "<html></html>", "image.jpg",
            Author.Create("teste", "test"), Category.Create("test", "test"), null));
        Assert.Throws<DomainException>(() => BlogPost.Create("id", "il grande post", "<html></html>", "image.jpg",
            Author.Create("teste", "test"), null, null));
        Assert.Throws<DomainException>(() =>
            BlogPost.Create("id", "il grande post", "<html></html>", null, null, null, null));
        Assert.Throws<DomainException>(() => BlogPost.Create("id", "il grande post", null, null, null, null, null));
        Assert.Throws<DomainException>(() => BlogPost.Create("id", null, null, null, null, null, null));
        Assert.Throws<DomainException>(() => BlogPost.Create(null, null, null, null, null, null, null));
    }

    [Fact]
    public void Update_BlogPost_WithValidData_Should_SetTheProps()
    {
        //SETUP
        var author = Author.Create("testId", "pippo");
        var category = Category.Create("categoryId", "musica");
        var tag = Tag.Create("tag1", "tag1");
        //ATTEMPT
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>", "image.jpg", author, category,
            new List<Tag> { tag });
        //VERIFY
        blogPost.Should().NotBeNull();
        blogPost.Update("il piccolo post", "test", "image2final.jpg");
        blogPost.Id.Should().Be("id");
        blogPost.Title.Should().Be("il piccolo post");
        blogPost.Content.Should().Be("test");
        blogPost.Image.Should().Be("image2final.jpg");
        blogPost.UpdateDateUtc.Should().BeAfter(DateTime.UtcNow.Date);
        author.BLogPosts.Should().BeEmpty();
    }

    [Fact]
    public void Associate_Tag_To_A_BlogPost__Should_NotThrow_Exceptions()
    {
        //SETUP
        var author = Author.Create("testId", "pippo");
        var category = Category.Create("categoryId", "musica");
        var tagOld = Tag.Create("tag1", "tag1");
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>", "image.jpg", author, category,
            new List<Tag> { tagOld });
        //ATTEMPT
        var tag = Tag.Create("testtag", "c#");
        //VERIFY
        blogPost.AssociateTag(tag);
        blogPost.TagXBlogPosts.Should().NotBeEmpty();
        blogPost.TagXBlogPosts.Any(x => x.TagId == tag.Id).Should().BeTrue();
    }

    [Fact]
    public void Associate_TheSameTag_To_A_BlogPost_Two_Times_Should_Throw_Exceptions()
    {
        //SETUP
        var author = Author.Create("testId", "pippo");
        var category = Category.Create("categoryId", "musica");
        var tagOld = Tag.Create("tag1", "tag1");
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>", "image.jpg", author, category,
            new List<Tag> { tagOld });
        var tag = Tag.Create("testtag", "c#");
        //ATTEMPT
        blogPost.AssociateTag(tag);
        //VERIFY
        Assert.Throws<DomainException>(() => blogPost.AssociateTag(tag));
    }

    [Fact]
    public void Associate_NullTag_To_A_BlogPost__Should_Throw_Exceptions()
    {
        //SETUP
        var author = Author.Create("testId", "pippo");
        var category = Category.Create("categoryId", "musica");
        var tagOld = Tag.Create("tag1", "tag1");
        //ATTEMPT
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>", "image.jpg", author, category,
            new List<Tag> { tagOld });
        //VERIFY
        Assert.Throws<DomainException>(() => blogPost.AssociateTag(null));
    }

    [Fact]
    public void Associate_Category_To_A_BlogPost__Should_NotThrow_Exceptions()
    {
        //SETUP
        var author = Author.Create("testId", "pippo");
        var categoryOld = Category.Create("categoryId", "musica");
        var tag = Tag.Create("tag1", "tag1");
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>", "image.jpg", author, categoryOld,
            new List<Tag> { tag });
        var category = Category.Create("testtag", "tech");
        //ATTEMPT
        blogPost.UpdateCategory(category);
        //VERIFY
        blogPost.CategoryId.Should().Be(category.Id);
    }

    [Fact]
    public void Associate_NullCategory_To_A_BlogPost__Should_Throw_Exceptions()
    {
        //SETUP
        var author = Author.Create("testId", "pippo");
        var category = Category.Create("categoryId", "musica");
        var tag = Tag.Create("tag1", "tag1");
        //ATTEMPT
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>", "image.jpg", author, category,
            new List<Tag> { tag });
        //VERIFY
        Assert.Throws<DomainException>(() => blogPost.UpdateCategory(null));
    }
}