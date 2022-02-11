using System;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Xunit;

namespace Blog.UnitTests;

public class BaseEntityTest : BaseEntity
{
    public BaseEntityTest(string? id, DateTime creationDateUtc) : base(id, creationDateUtc)
    {
    }

    public void CheckDate(DateTime? date)
    {
        IsValidDate(date, nameof(date));
        ValidateErrors();
    }

    public void CheckGuid(Guid guid)
    {
        IsInvalidGuid(guid, nameof(guid));
        ValidateErrors();
    }

    public void CheckString(string str)
    {
        IsInvalidString(str, nameof(str));
        ValidateErrors();
    }

    public void CheckInt(int value)
    {
        IsInvalidInt(value, nameof(value));
        ValidateErrors();
    }
}

public class BaseEntityUnitTest
{
    [Fact]
    public void BaseEntity_Should_Not_Allow_InstanceCreation_WithInvalid()
    {
        Assert.Throws<DomainException>(() => new BaseEntityTest(null, DateTime.Now));
        Assert.Throws<DomainException>(() => new BaseEntityTest("sdfdsf", new DateTime(1990, 01, 01)));
    }

    [Fact]
    public void InvalidGuid_Should_Throw_A_DomainException()
    {
        var test = new BaseEntityTest("test", DateTime.UtcNow);
        Assert.Throws<DomainException>(() => test.CheckGuid(Guid.Empty));
    }

    [Fact]
    public void InvalidString_Should_Throw_A_DomainException()
    {
        var test = new BaseEntityTest("test", DateTime.UtcNow);
        Assert.Throws<DomainException>(() => test.CheckString(null));
        Assert.Throws<DomainException>(() => test.CheckString(""));
        Assert.Throws<DomainException>(() => test.CheckString("     "));
    }

    [Fact]
    public void InvalidInt_Should_Throw_A_DomainException()
    {
        var test = new BaseEntityTest("test", DateTime.UtcNow);
        Assert.Throws<DomainException>(() => test.CheckInt(0));
    }

    [Fact]
    public void InvalidDate_Should_Throw_A_DomainException()
    {
        var test = new BaseEntityTest("test", DateTime.UtcNow);
        Assert.Throws<DomainException>(() => test.CheckDate(null));
        var date = new DateTime(2020, 01, 01, 01, 01, 01, DateTimeKind.Unspecified);
        Assert.Throws<DomainException>(() => test.CheckDate(date));
    }
}