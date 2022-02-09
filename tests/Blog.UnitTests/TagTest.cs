using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Blog.UnitTests;

public class TagTest
{
    [Fact]
    public void Create_New_Tag_WithValidDate_Should_NotThrow_Exceptions()
    {
        string? id = "testId";
        string? name = "c#";
        var category = Tag.Create(id, name);
        category.Should().NotBeNull();
        category.Id.Should().Be(id);
        category.Name.Should().Be(name);
    }
    [Fact]
    public void Create_New_Category_WithInvalidData_Should_Throw_Exceptions()
    {
        Assert.Throws<DomainException>((() => Tag.Create(null, null)));
        Assert.Throws<DomainException>((() => Tag.Create("sdfdsfdsf", null)));
        Assert.Throws<DomainException>((() => Tag.Create(null, "sdfdsfdfdsf")));
    }
}