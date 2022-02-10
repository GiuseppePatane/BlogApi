using System;
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
        var author = Author.Create("testId", "pippo");
        var category = Category.Create("categoryId", "musica");
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>","image.jpg",author,category);
        blogPost.Should().NotBeNull();
        blogPost.Id.Should().Be("id");
        blogPost.Title.Should().Be("il grande post");
        blogPost.Content.Should().Be("<html></html>");
        blogPost.Image.Should().Be("image.jpg");
        blogPost.CategoryId.Should().Be(category.Id);
        blogPost.AuthorId.Should().Be(author.Id);
        author.BLogPosts.Should().BeEmpty();
    }
    
    [Fact]
    public void Create_New_BlogPost_WithInValidData_Should_Throw_Exceptions()
    {
        Assert.Throws<DomainException>((() => BlogPost.Create("id", "il grande post", "<html></html>","image.jpg",Author.Create("teste","test"), null)));
        Assert.Throws<DomainException>((() => BlogPost.Create("id", "il grande post", "<html></html>",null,null,null)));
        Assert.Throws<DomainException>((() => BlogPost.Create("id", "il grande post", null,null,null,null)));
        Assert.Throws<DomainException>((() => BlogPost.Create("id", null, null,null,null,null)));
        Assert.Throws<DomainException>((() => BlogPost.Create(null, null, null,null,null,null)));
    }
    
    [Fact]
    public void Update_BlogPost_WithValidData_Should_SetTheProps()
    {
        var author = Author.Create("testId", "pippo");
        var category = Category.Create("categoryId", "musica");
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>","image.jpg",author,category);
        blogPost.Should().NotBeNull();
        blogPost.Update("il piccolo post","test","image2final.jpg");
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
        var author = Author.Create("testId", "pippo");
        var category = Category.Create("categoryId", "musica");
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>","image.jpg",author,category);
        var tag = Tag.Create("testtag", "c#");
        blogPost.AssociateWithTag(tag);
        blogPost.TagXBlogPosts.Should().NotBeEmpty();
        blogPost.TagXBlogPosts.Any(x => x.TagId == tag.Id).Should().BeTrue();
    }
    [Fact]
    public void Associate_NullTag_To_A_BlogPost__Should_Throw_Exceptions()
    {
        var author = Author.Create("testId", "pippo");
        var category = Category.Create("categoryId", "musica");
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>","image.jpg",author,category);
        var tag = Tag.Create("testtag", "c#");
        blogPost.AssociateWithTag(tag);
        Assert.Throws<DomainException>((() => blogPost.AssociateWithTag(tag)));
    }
    [Fact]
    public void Associate_TheSameTag_To_A_BlogPost_Two_Times_Should_Throw_Exceptions()
    {
        var author = Author.Create("testId", "pippo");
        var category = Category.Create("categoryId", "musica");
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>","image.jpg",author,category);
        Assert.Throws<DomainException>((() => blogPost.AssociateWithTag(null)));
    }
    [Fact]
    public void Associate_Category_To_A_BlogPost__Should_NotThrow_Exceptions()
    {
        var author = Author.Create("testId", "pippo");
        var categoryOld = Category.Create("categoryId", "musica");
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>","image.jpg",author,categoryOld);
        var category = Category.Create("testtag", "tech");
        blogPost.AssociateWithCategory(category);
        blogPost.CategoryId.Should().Be(category.Id);
    }
    [Fact]
    public void Associate_NullCategory_To_A_BlogPost__Should_Throw_Exceptions()
    {
        var author = Author.Create("testId", "pippo");
        var category = Category.Create("categoryId", "musica");
        var blogPost = BlogPost.Create("id", "il grande post", "<html></html>","image.jpg",author,category);
        Assert.Throws<DomainException>((() => blogPost.AssociateWithCategory(null)));
    }
}