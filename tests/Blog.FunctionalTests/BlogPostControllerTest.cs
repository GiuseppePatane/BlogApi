using System.Net.Http.Json;
using System.Threading.Tasks;
using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Blog.FunctionalTests;

public class BlogPostControllerTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public BlogPostControllerTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    [Fact]
    public async Task CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId()
    {
        var request = new
        {
            Url = "/api/BlogPost",
            Body = new
            {
                name = "test",
                title= "test", 
                content= "string",
                image= "string",
                authorId= "test",
                categoryId= "newCategory", 
                tags= new string[]
                {
                    "test"
                }
            }
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("newCategory", "music");
        context.Categories.Add(category);
        context.Authors.Add(author);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        var response =  await  client.PostAsJsonAsync(request.Url, request.Body);
        response.IsSuccessStatusCode.Should().BeTrue();
        var model = await response.Content.ReadFromJsonAsync<CreateResponse>();
        model.Should().NotBeNull();
        model.Id.Should().NotBeNull();
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
}