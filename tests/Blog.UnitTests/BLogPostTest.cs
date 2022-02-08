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
        var bLogPost = BLogPost.Create("id", "il grande post", "<html></html>","image.jpg",author);
        bLogPost.Should().NotBeNull();
        bLogPost.Id.Should().Be("id");
        bLogPost.Title.Should().Be("il grande post");
        bLogPost.Content.Should().Be("<html></html>");
        bLogPost.Image.Should().Be("image.jpg");
        author.BLogPosts.Should().BeEmpty();
    }
    
    [Fact]
    public void Create_New_BlogPost_WithInValidData_Should_Throw_Exceptions()
    {
        Assert.Throws<DomainException>((() => BLogPost.Create("id", "il grande post", "<html></html>","image.jpg",null)));
        Assert.Throws<DomainException>((() => BLogPost.Create("id", "il grande post", "<html></html>",null,null)));
        Assert.Throws<DomainException>((() => BLogPost.Create("id", "il grande post", null,null,null)));
        Assert.Throws<DomainException>((() => BLogPost.Create("id", null, null,null,null)));
        Assert.Throws<DomainException>((() => BLogPost.Create(null, null, null,null,null)));
    }
    
    [Fact]
    public void Associate_Tag_To_A_BlogPost__Should_NotThrow_Exceptions()
    {
        var author = Author.Create("testId", "pippo");
        var bLogPost = BLogPost.Create("id", "il grande post", "<html></html>","image.jpg",author);
        var tag = Tag.Create("testtag", "c#");
        bLogPost.AssociateWithTag(tag);
        bLogPost.TagXBlogPosts.Should().NotBeEmpty();
        bLogPost.TagXBlogPosts.Any(x => x.TagId == tag.Id).Should().BeTrue();
    }
    [Fact]
    public void Associate_NullTag_To_A_BlogPost__Should_Throw_Exceptions()
    {
        var author = Author.Create("testId", "pippo");
        var bLogPost = BLogPost.Create("id", "il grande post", "<html></html>","image.jpg",author);
        Assert.Throws<DomainException>((() => bLogPost.AssociateWithTag(null)));
    }
    [Fact]
    public void Associate_Category_To_A_BlogPost__Should_NotThrow_Exceptions()
    {
        var author = Author.Create("testId", "pippo");
        var bLogPost = BLogPost.Create("id", "il grande post", "<html></html>","image.jpg",author);
        var category = Category.Create("testtag", "tech");
        bLogPost.AssociateWithCategory(category);
        bLogPost.CategoryId.Should().Be(category.Id);
    }
    [Fact]
    public void Associate_NullCategory_To_A_BlogPost__Should_Throw_Exceptions()
    {
        var author = Author.Create("testId", "pippo");
        var bLogPost = BLogPost.Create("id", "il grande post", "<html></html>","image.jpg",author);
        Assert.Throws<DomainException>((() => bLogPost.AssociateWithCategory(null)));
    }
}