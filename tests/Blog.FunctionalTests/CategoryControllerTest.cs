using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using FluentAssertions;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Blog.FunctionalTests;

    [UsesVerify]
    public class CategoryControllerTest
    {
        private ITestOutputHelper _testOutputHelper;

        public CategoryControllerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task CreateNewCategory_WithValidRequest_ShouldReturnTheCreatedId()
        {
            //SETUP
            var request = new
            {
                Url = "/api/Category",
                Body = new
                {
                    name = "test",
                }
            };
            await using var context = TestClient.GetDbContext(
                nameof(this.CreateNewCategory_WithValidRequest_ShouldReturnTheCreatedId), out var connectionString);
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
        public async Task CreateNewCategory_TwoTimes_WithValidRequest_ShouldReturnTheCreatedId()
        {
            //SETUP
            var request = new
            {
                Url = "/api/Category",
                Body = new
                {
                    name = "test",
                }
            };
            await using var context = TestClient.GetDbContext(
                nameof(this.CreateNewCategory_WithValidRequest_ShouldReturnTheCreatedId), out var connectionString);
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
        [Fact]
        public async Task GetCategories_WithValidQueryStringParameters_ShouldReturnTheCorrectElements()
        {
            //SETUP
            var request = new
            {
                Url = "/api/Categories?Name=test",
            };
            await using var context = TestClient.GetDbContext(nameof(this.GetCategories_WithValidQueryStringParameters_ShouldReturnTheCorrectElements),out var connectionString);
            await TestClient.PrepareDatabase(context);
            List<Category> categories = new List<Category>();
            for (int i = 1; i <= 30; i++)
            {
                categories.Add(Category.Create($"{i}", $"test{i}"));
            }
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
            var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
            client.DefaultRequestHeaders.Add("X-USER", "user");
            //ATTEMPT
            var response = await client.GetAsync(request.Url);
            //VERIFY
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var model = await response.Content.ReadFromJsonAsync<CategoryPaginationResponse>();
            await Verifier.Verify(model,TestClient.GetVerifySettings());
            await TestClient.CheckDatabaseAndRemoveIt(context);
        }
        [Fact]
        public async Task GetCategories_WithNotExistingName_ShouldReturnNoContent()
        {
            //SETUP
            var request = new
            {
                Url = "/api/Categories?name=dsfdsfdsfsdf",
            };
            await using var context = TestClient.GetDbContext(nameof(this.GetCategories_WithNotExistingName_ShouldReturnNoContent),out var connectionString);
            await TestClient.PrepareDatabase(context);
            var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
            client.DefaultRequestHeaders.Add("X-USER", "user");
            //ATTEMPT
            var response = await client.GetAsync(request.Url);
            //VERIFY
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            await TestClient.CheckDatabaseAndRemoveIt(context);
        }
    }