using System;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Blog.UnitTests.Entities;

public class CategoryTest
{
    [Fact]
    public void Create_New_Category_WithValidDate_Should_Throw_Exceptions()
    {
        //SETUP
        string? id = "testId";
        string? name = "programming";
        //ATTEMPT
        var category = Category.Create(id, name);
        //VERIFU
        category.Should().NotBeNull();
        category.Id.Should().Be(id);
        category.Name.Should().Be(name);
        category.CreationDateUtc.Should().BeAfter(DateTime.UtcNow.Date);
    }
    
    [Fact]
    public void Create_New_Category_WithInvalidData_Should_NotThrow_Exceptions()
    {
        Assert.Throws<DomainException>((() => Category.Create(null, null)));
        Assert.Throws<DomainException>((() => Category.Create("sdfdsfdsf", null)));
        Assert.Throws<DomainException>((() => Category.Create(null, "sdfdsfdfdsf")));
    }
    
    [Fact]
    public void Update_Category_WithValidDate_Should_NotThrow_Exceptions()
    {
        //SETUP
        string? id = "testId";
        string? name = "programming";
        //ATTEMPT
        var category = Category.Create(id, name);
        //VERIFY
        category.Should().NotBeNull();
        category.Id.Should().Be(id);
        category.Name.Should().Be(name);
        category.Update("letteracture");
        category.Name.Should().Be("letteracture");
        category.UpdateDateUtc.Should().BeAfter(DateTime.UtcNow.Date);
    }
}