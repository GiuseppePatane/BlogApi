using Blog.Domain.Helpers;
using FluentAssertions;
using Xunit;

namespace Blog.UnitTests;

public class ValidatorTest
{
    [Theory]
    [InlineData("",false)]
    [InlineData("     ",false)]
    [InlineData("sdfdsfdsfsdfdsdsfds",false)]
    [InlineData("https://www.google.com/",false)]
    [InlineData("http://mycdn.com/.",false)]
    [InlineData("http://mycdn.com/image.jpg",true)]
    public void FileValidation_ShouldReturn_TheExpectedResult(string url,bool expectedResult)
    {
        var isFile = ValidationsHelper.IsFile(url);
        isFile.Should().Be(expectedResult);
    }
}