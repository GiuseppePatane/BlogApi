using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using FluentAssertions;
using Newtonsoft.Json;
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
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost",
            Body = new
            {
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
        var tag = Tag.Create("test", "test");
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response =  await  client.PostAsJsonAsync(request.Url, request.Body);
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var model = await response.Content.ReadFromJsonAsync<CreateResponse>();
        model.Should().NotBeNull();
        model.Id.Should().NotBeNull();
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
    
    [Fact]
    public async Task CreateNewBlogPost_WithExistingBlogPost_ShouldReturnABadRequest()
    {
        //SETUP
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
        var tag = Tag.Create("test", "test");
        var blogPost = BlogPost.Create("test", "test","string","image",author,category,new List<Tag>(){tag});
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response =  await  client.PostAsJsonAsync(request.Url, request.Body);
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Should().NotBeNull();
        var error = model.Errors.FirstOrDefault();
        error.Message.Should().Be("blog post already exist");
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }

    [Fact]
    public async Task Delete_ExistingBlogPost_ShouldReturnAnOkResponse()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/testDelete",
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("newCategory", "music");
        var tag = Tag.Create("test", "test");
        var blogPost = BlogPost.Create("testDelete", "test","string","image",author,category,new List<Tag>(){tag});
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "admin");
       var response= await client.DeleteAsync(request.Url);
       response.StatusCode.Should().Be(HttpStatusCode.OK);
       var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
       model.Should().NotBeNull();
       model.Errors.Any().Should().BeFalse();
    }
    
    [Fact]
    public async Task Delete_NotExistingBlogPost_ShouldReturnABadRequest()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/testDelete",
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("newCategory", "music");
        var tag = Tag.Create("test", "test");
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "admin");
        var response= await client.DeleteAsync(request.Url);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Any().Should().BeTrue();
        var error = model.Errors.FirstOrDefault();
        error.Should().NotBeNull();
        error.Code.Should().Be("DomainExceptionKey");
        error.Message.Should().Be("blog post not found");
    }
    
    [Fact]
    public async Task Delete_ExistingBlogPost_WithNotAdminXUser_ShouldReturnAForbiddenResult()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/testDelete",
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("newCategory", "music");
        var tag = Tag.Create("test", "test");
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        var response= await client.DeleteAsync(request.Url);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    [Fact]
    public async Task Edit_ExistingBlogPost_ShouldReturnAnOkResponse()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/1",
            Body = new
            {
                title= "string2", 
                content= "string2",
                image= "string2",
            }
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("newCategory", "music");
        var tag = Tag.Create("test", "test");
        var blogPost = BlogPost.Create("1", "test","string","image",author,category,new List<Tag>(){tag});
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response =  await  client.PatchAsync(request.Url, new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.UTF8, "application/json") );
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Any().Should().BeFalse();
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
    
    [Fact]
    public async Task Edit_NotExistingBlogPost_ShouldReturnABadRequest()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/sdfsfsf",
            Body = new
            {
                title= "string2", 
                content= "string2",
                image= "string2",
            }
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response =  await  client.PatchAsync(request.Url, new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.UTF8, "application/json") );
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Any().Should().BeTrue();
        var error = model.Errors.FirstOrDefault();
        error.Should().NotBeNull();
        error.Code.Should().Be("DomainExceptionKey");
        error.Message.Should().Be("blog post not found");
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
    
    
    [Fact]
    public async Task Update_Category_Of_A_Valid_BlogPost_WithAValidCategory_ShouldReturnOk()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/1/Category/CategoryNew",
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("Category", "music");
        var categoryNew = Category.Create("CategoryNew", "fantasy");
        var tag = Tag.Create("test", "test");
        var blogPost = BlogPost.Create("1", "test","string","image",author,category,new List<Tag>(){tag});
        context.Categories.Add(category);
        context.Categories.Add(categoryNew);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response =  await  client.SendAsync( new HttpRequestMessage(new HttpMethod("PATCH"), request.Url));
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Any().Should().BeFalse();
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
    
    [Fact]
    public async Task Update_Category_Of_A_NotExisting_BlogPost_WithAValidCategory_ShouldReturnBadRequest()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/1/Category/CategoryNew",
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("Category", "music");
        var categoryNew = Category.Create("CategoryNew", "fantasy");
        var tag = Tag.Create("test", "test");
        context.Categories.Add(category);
        context.Categories.Add(categoryNew);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response =  await  client.SendAsync( new HttpRequestMessage(new HttpMethod("PATCH"), request.Url));
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Any().Should().BeTrue();
        var error = model.Errors.FirstOrDefault();
        error.Should().NotBeNull();
        error.Code.Should().Be("DomainExceptionKey");
        error.Message.Should().Be("blog post not found");
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
    
    [Fact]
    public async Task Update_Category_Of_A_Valid_BlogPost_WithANotExistingCategory_ShouldReturnBadRequest()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/1/Category/CategoryNew",
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("Category", "music");
    
        var tag = Tag.Create("test", "test");
        var blogPost = BlogPost.Create("1", "test","string","image",author,category,new List<Tag>(){tag});
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response =  await  client.SendAsync( new HttpRequestMessage(new HttpMethod("PATCH"), request.Url));
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Any().Should().BeTrue();
        var error = model.Errors.FirstOrDefault();
        error.Should().NotBeNull();
        error.Code.Should().Be("DomainExceptionKey");
        error.Message.Should().Be("category not found");
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
    
    
    [Fact]
    public async Task AddTag_With_A_Valid_BlogPost_WithAExistingTag_ShouldReturnOk()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/1/Tags/new",
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("Category", "music");
        var tag = Tag.Create("test", "test");
        var tagNew = Tag.Create("new", "test2");
        var blogPost = BlogPost.Create("1", "test","string","image",author,category,new List<Tag>(){tag});
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        context.Tags.Add(tagNew);
        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response =  await  client.SendAsync( new HttpRequestMessage(new HttpMethod("PATCH"), request.Url));
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Any().Should().BeFalse();
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
    
    [Fact]
    public async Task AddTag_With_A_NotExisting_BlogPost_ShouldReturnBadRequest()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/1/Tags/new",
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("Category", "music");
        var tag = Tag.Create("test", "test");
        var tagNew = Tag.Create("new", "test2");
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        context.Tags.Add(tagNew);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response =  await  client.SendAsync( new HttpRequestMessage(new HttpMethod("PATCH"), request.Url));
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Any().Should().BeTrue();
        var error = model.Errors.FirstOrDefault();
        error.Should().NotBeNull();
        error.Code.Should().Be("DomainExceptionKey");
        error.Message.Should().Be("blog post not found");
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
    
    [Fact]
    public async Task AddNotExistingTag_With_A_Existing_BlogPost_ShouldReturnBadRequest()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/1/Tags/new",
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithValidRequest_ShouldReturnTheCreatedId),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("Category", "music");
    
        var tag = Tag.Create("test", "test");
        var blogPost = BlogPost.Create("1", "test","string","image",author,category,new List<Tag>(){tag});
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        context.BlogPosts.Add(blogPost);
        await context.SaveChangesAsync();
        
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response =  await  client.SendAsync( new HttpRequestMessage(new HttpMethod("PATCH"), request.Url));
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Any().Should().BeTrue();
        var error = model.Errors.FirstOrDefault();
        error.Should().NotBeNull();
        error.Code.Should().Be("DomainExceptionKey");
        error.Message.Should().Be("tag not found");
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
}