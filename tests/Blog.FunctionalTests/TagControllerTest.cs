using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blog.Domain.DTOs;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Blog.FunctionalTests;

public class TagControllerTest
{
    private ITestOutputHelper _testOutputHelper;

    public TagControllerTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CreateNewTag_WithValidRequest_ShouldReturnTheCreatedId()
    {
        //SETUP
        var request = new
        {
            Url = "/api/Tag",
            Body = new
            {
                name = "test",
            }
        };
        await using var context = TestClient.GetDbContext(
            nameof(this.CreateNewTag_WithValidRequest_ShouldReturnTheCreatedId), out var connectionString);
        await TestClient.PrepareDatabase(context);
        var client = TestClient.CreateHttpClient(_testOutputHelper, connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response = await client.PostAsJsonAsync(request.Url, request.Body);
        //VERIFY
        response.IsSuccessStatusCode.Should().BeTrue();
        var model = await response.Content.ReadFromJsonAsync<CreateResponse>();
        model.Should().NotBeNull();
        model.Id.Should().NotBeNull();
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }

    [Fact]
    public async Task CreateNewTag_TwoTimes_WithValidRequest_ShouldReturnTheCreatedId()
    {
        //SETUP
        var request = new
        {
            Url = "/api/Tag",
            Body = new
            {
                name = "test",
            }
        };
        await using var context = TestClient.GetDbContext(
            nameof(this.CreateNewTag_WithValidRequest_ShouldReturnTheCreatedId), out var connectionString);
        await TestClient.PrepareDatabase(context);
        var client = TestClient.CreateHttpClient(_testOutputHelper, connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response = await client.PostAsJsonAsync(request.Url, request.Body);
        //VERIFY
        response.IsSuccessStatusCode.Should().BeTrue();
        var model = await response.Content.ReadFromJsonAsync<CreateResponse>();
        model.Should().NotBeNull();
        model.Id.Should().NotBeNull();
        var errorResponse = await client.PostAsJsonAsync(request.Url, request.Body);
        errorResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var errorModel = await errorResponse.Content.ReadFromJsonAsync<ErrorResponse>();

        var error = errorModel.Errors.FirstOrDefault();
        error.Should().NotBeNull();
        error.Code.Should().Be("DomainExceptionKey");
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }



}