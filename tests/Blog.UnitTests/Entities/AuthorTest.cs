using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Blog.UnitTests.Entities;

public class AuthorTest
{
    [Fact]
    public void Create_New_Author_WithValidDate_Should_NotThrow_Exceptions()
    {
        //SETUP
        string? id = "testId";
        string? name = "Pippo";
        //ATTEMPT
        var author = Author.Create(id, name);
        //VERIFY
        author.Should().NotBeNull();
        author.Id.Should().Be(id);
        author.Name.Should().Be(name);
        author.BLogPosts.Should().BeEmpty();
    }
    
    [Fact]
    public void Create_New_Author_WithInvalidData_Should_Throw_Exceptions()
    {
        Assert.Throws<DomainException>((() => Author.Create(null, null)));
        Assert.Throws<DomainException>((() => Author.Create("dsfdsfdsfsd", null)));
        Assert.Throws<DomainException>((() => Author.Create(null, "dfsdfsfdsfsd")));
    }
}