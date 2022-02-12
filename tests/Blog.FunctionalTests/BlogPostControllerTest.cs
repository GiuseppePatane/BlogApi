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
using VerifyTests;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Blog.FunctionalTests;

[UsesVerify]
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
                imageUrl= "https://images.agi.it/pictures/agi/agi/2021/11/15/143214429-dd3e4ea6-2844-48b3-a099-a917bbb27d52.jpg",
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
    public async Task CreateNewBlogPost_WithLargeContent_ShouldReturnBadRequest()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost",
            Body = new
            {
                title= "test", 
                content= ";Zw8&C}87Z44{UxD$[#e(NW:?]qtB.[WbzS#eXpY!L7MzL]}V-YxFnc&iF{z4r:&=ZQ8k+$B+C;;ND;wwD?-cnmg/X)NH{-7@w$+*fMma{f!N&t}P2zvEES+SfTFS#%YSN$zt[$j?RE)?nn%_$uNx9W5,Pd[DQuq?Qmp7=6t]7{+k$NDSg+=C][N%zu#wUqKzuVi;a*!kJcmb6#B%T#[gAap_QVi9jfyqTHxCmxi8EbT;/b:.eH+/g$YEDt%$-+%5uTZwviwE-]{xRNhM=d:TREaeG(g/;eBq9[&RxN$}?=]d*QWkdJ&2;f(@FJ-&T?vmRP4LVY$Fv(9Nk&KEGSYd3;yx-3B?d3+2eAB@]wp=(WKtP[VgE=]%MT}hhS7!b96$@3L;m]_-Jetf;d9a]MWB/UnB;inGW#,dq.5[!,c7nG9M7}z5K/mSHf4w(q&KAyByn(m+pQviT[CPrm+2NFk7f]]z&)G]26RfB/vt5x8?]FYV@&[ZtgiZq{6c[*q$m)4,R-ibE!+XiF-hRdR@KYjLu6P?$+y,6En?=SHBENw!Nd%%mw!/yu}Vp($R.Z{%}cjfKunv$EK!r4hENJN[S+3)Nr.U9x[,yJ_$EzZ6w*_GhD)Bh7;8Lf=/,h9Adk;{&E2mnnaa#@[8(Utn{e&{2-!)SSEaqAKe=.UXzC2Jy%9J),GXe;/9Hd2u@3Z7ArchR#wnbq98+b#HJyW-!SDYj3FEk:+:m$jS{tWt/(GX*PL6:v8p&,U&w@/7[DjXxC-TV}vjW@E8.7SWHG{w#.jrVp)AG%N=&yWdeKH}YSepa$DGmdLjxUgC{V$CXV)hVamkBd4(a%_W&m4-y$k@)PeS8k[vV5?n2)!#hA##?:fWbCcA({2QV!tJ5w%f.F:!3Jn8u_G#RuXiQ8;eU*BG+mn/?{5-G}Bt8LCd5D[kKvh,D@UqNC.nVDZk7@H$XAfh@_wH9LiyEhA;HHBfr;wqzXtfg[M;jJu))2jX9]xH655UJ;c%P$P8)7b(;xndw58ieqFQ@#-?",
                imageUrl= "https://images.agi.it/pictures/agi/agi/2021/11/15/143214429-dd3e4ea6-2844-48b3-a099-a917bbb27d52.jpg",
                authorId= "test",
                categoryId= "newCategory", 
                tags= new string[]
                {
                    "test"
                }
            }
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithLargeContent_ShouldReturnBadRequest),out var connectionString);
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
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Count.Should().Be(1);
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
    [Fact]
    public async Task CreateNewBlogPost_WithInvalidRequest_ShouldReturnABadRequest()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost",
            Body = new
            {
                tags= new string[]
                {
                    
                }
            }
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithInvalidRequest_ShouldReturnABadRequest),out var connectionString);
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
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Count.Should().Be(12);
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
                imageUrl= "https://images.agi.it/pictures/agi/agi/2021/11/15/143214429-dd3e4ea6-2844-48b3-a099-a917bbb27d52.jpg",
                authorId= "test",
                categoryId= "newCategory", 
                tags= new string[]
                {
                    "test"
                }
            }
        };
        await using var context = TestClient.GetDbContext(nameof(this.CreateNewBlogPost_WithExistingBlogPost_ShouldReturnABadRequest),out var connectionString);
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
        await using var context = TestClient.GetDbContext(nameof(this.Delete_ExistingBlogPost_ShouldReturnAnOkResponse),out var connectionString);
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
        await using var context = TestClient.GetDbContext(nameof(this.Delete_NotExistingBlogPost_ShouldReturnABadRequest),out var connectionString);
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
        await using var context = TestClient.GetDbContext(nameof(this.Delete_ExistingBlogPost_WithNotAdminXUser_ShouldReturnAForbiddenResult),out var connectionString);
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
                imageUrl= "https://images.agi.it/pictures/agi/agi/2021/11/15/143214429-dd3e4ea6-2844-48b3-a099-a917bbb27d52.jpg",
            }
        };
        await using var context = TestClient.GetDbContext(nameof(this.Edit_ExistingBlogPost_ShouldReturnAnOkResponse),out var connectionString);
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
    public async Task Edit_WithInvalidParameters_ShouldReturnABadRequest()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/sdfsfsf",
            Body = new
            {
                title= "string2", 
                content= ";Zw8&C}87Z44{UxD$[#e(NW:?]qtB.[WbzS#eXpY!L7MzL]}V-YxFnc&iF{z4r:&=ZQ8k+$B+C;;ND;wwD?-cnmg/X)NH{-7@w$+*fMma{f!N&t}P2zvEES+SfTFS#%YSN$zt[$j?RE)?nn%_$uNx9W5,Pd[DQuq?Qmp7=6t]7{+k$NDSg+=C][N%zu#wUqKzuVi;a*!kJcmb6#B%T#[gAap_QVi9jfyqTHxCmxi8EbT;/b:.eH+/g$YEDt%$-+%5uTZwviwE-]{xRNhM=d:TREaeG(g/;eBq9[&RxN$}?=]d*QWkdJ&2;f(@FJ-&T?vmRP4LVY$Fv(9Nk&KEGSYd3;yx-3B?d3+2eAB@]wp=(WKtP[VgE=]%MT}hhS7!b96$@3L;m]_-Jetf;d9a]MWB/UnB;inGW#,dq.5[!,c7nG9M7}z5K/mSHf4w(q&KAyByn(m+pQviT[CPrm+2NFk7f]]z&)G]26RfB/vt5x8?]FYV@&[ZtgiZq{6c[*q$m)4,R-ibE!+XiF-hRdR@KYjLu6P?$+y,6En?=SHBENw!Nd%%mw!/yu}Vp($R.Z{%}cjfKunv$EK!r4hENJN[S+3)Nr.U9x[,yJ_$EzZ6w*_GhD)Bh7;8Lf=/,h9Adk;{&E2mnnaa#@[8(Utn{e&{2-!)SSEaqAKe=.UXzC2Jy%9J),GXe;/9Hd2u@3Z7ArchR#wnbq98+b#HJyW-!SDYj3FEk:+:m$jS{tWt/(GX*PL6:v8p&,U&w@/7[DjXxC-TV}vjW@E8.7SWHG{w#.jrVp)AG%N=&yWdeKH}YSepa$DGmdLjxUgC{V$CXV)hVamkBd4(a%_W&m4-y$k@)PeS8k[vV5?n2)!#hA##?:fWbCcA({2QV!tJ5w%f.F:!3Jn8u_G#RuXiQ8;eU*BG+mn/?{5-G}Bt8LCd5D[kKvh,D@UqNC.nVDZk7@H$XAfh@_wH9LiyEhA;HHBfr;wqzXtfg[M;jJu))2jX9]xH655UJ;c%P$P8)7b(;xndw58ieqFQ@#-?",
                imageUrl= "https://",
            }
        };
        await using var context = TestClient.GetDbContext(nameof(this.Edit_WithInvalidParameters_ShouldReturnABadRequest),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response =  await  client.PatchAsync(request.Url, new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.UTF8, "application/json") );
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var model = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        model.Should().NotBeNull();
        model.Errors.Count.Should().Be(2);
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
                imageUrl= "https://images.agi.it/pictures/agi/agi/2021/11/15/143214429-dd3e4ea6-2844-48b3-a099-a917bbb27d52.jpg",
            }
        };
        await using var context = TestClient.GetDbContext(nameof(this.Edit_NotExistingBlogPost_ShouldReturnABadRequest),out var connectionString);
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
        await using var context = TestClient.GetDbContext(nameof(this.Update_Category_Of_A_Valid_BlogPost_WithAValidCategory_ShouldReturnOk),out var connectionString);
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
        await using var context = TestClient.GetDbContext(nameof(this.Update_Category_Of_A_NotExisting_BlogPost_WithAValidCategory_ShouldReturnBadRequest),out var connectionString);
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
        await using var context = TestClient.GetDbContext(nameof(this.Update_Category_Of_A_Valid_BlogPost_WithANotExistingCategory_ShouldReturnBadRequest),out var connectionString);
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
        await using var context = TestClient.GetDbContext(nameof(this.AddTag_With_A_Valid_BlogPost_WithAExistingTag_ShouldReturnOk),out var connectionString);
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
        await using var context = TestClient.GetDbContext(nameof(this.AddTag_With_A_NotExisting_BlogPost_ShouldReturnBadRequest),out var connectionString);
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
        await using var context = TestClient.GetDbContext(nameof(this.AddNotExistingTag_With_A_Existing_BlogPost_ShouldReturnBadRequest),out var connectionString);
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
    [Fact]
    public async Task GetBlogPost_WithValidId_ShouldReturnTheCorrectElements()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/1",
        };
        await using var context = TestClient.GetDbContext(nameof(this.GetBlogPost_WithValidId_ShouldReturnTheCorrectElements),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("Category", "music");
        var tag = Tag.Create("test", "test");
        var blogPostList = new List<BlogPost>();
        var content = @"<!DOCTYPE html>
                        <html>
                        <head>
                        <title>Page Title</title>
                        </head>
                        <body>

                        <h1>My First Heading</h1>
                        <p>My first paragraph.</p>

                        </body>
                        </html>
                        ";
        for (int i = 1; i <= 30; i++)
        {
            blogPostList.Add(BlogPost.Create($"{i}", $"test{i}",content,"https://images.agi.it/pictures/agi/agi/2021/11/15/143214429-dd3e4ea6-2844-48b3-a099-a917bbb27d52.jpg",author,category,new List<Tag>(){tag}));
        }
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        context.BlogPosts.AddRange(blogPostList);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response = await client.GetAsync(request.Url);
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var model = await response.Content.ReadFromJsonAsync<BlogPostResponse>();
        await Verifier.Verify(model,TestClient.GetVerifySettings());
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
 [Fact]
    public async Task GetBlogPost_WithInvalidId_ShouldReturnNoContent()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPost/1",
        };
        await using var context = TestClient.GetDbContext(nameof(this.GetBlogPost_WithValidId_ShouldReturnTheCorrectElements),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response = await client.GetAsync(request.Url);
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
    
    [Fact]
    public async Task GetBlogPosts_WithValidQueryStringParameters_ShouldReturnTheCorrectElements()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPosts?category=music&tags=test",
        };
        await using var context = TestClient.GetDbContext(nameof(this.GetBlogPosts_WithValidQueryStringParameters_ShouldReturnTheCorrectElements),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("Category", "music");
        var tag = Tag.Create("test", "test");
        var blogPostList = new List<BlogPost>();
        var content = @"<!DOCTYPE html>
                        <html>
                        <head>
                        <title>Page Title</title>
                        </head>
                        <body>

                        <h1>My First Heading</h1>
                        <p>My first paragraph.</p>

                        </body>
                        </html>
                        ";
        for (int i = 1; i <= 30; i++)
        {
            blogPostList.Add(BlogPost.Create($"{i}", $"test{i}",content,"https://images.agi.it/pictures/agi/agi/2021/11/15/143214429-dd3e4ea6-2844-48b3-a099-a917bbb27d52.jpg",author,category,new List<Tag>(){tag}));
        }
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.Add(tag);
        context.BlogPosts.AddRange(blogPostList);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response = await client.GetAsync(request.Url);
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var model = await response.Content.ReadFromJsonAsync<BlogPostPaginationResponse>();
        await Verifier.Verify(model,TestClient.GetVerifySettings());
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
     [Fact]
    public async Task GetBlogPosts_WithMultipleTagQueryStringParameters_ShouldReturnTheCorrectElements()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPosts?tags=test&tags=prova",
        };
        await using var context = TestClient.GetDbContext(nameof(this.GetBlogPosts_WithMultipleTagQueryStringParameters_ShouldReturnTheCorrectElements),out var connectionString);
        await TestClient.PrepareDatabase(context);
        var author = Author.Create("test", "grande scrittore");
        var category = Category.Create("Category", "music");
        var tag = Tag.Create("test", "test");
        var tag2 = Tag.Create("prova", "prova");
        var blogPostList = new List<BlogPost>();
        var content = @"<!DOCTYPE html>
                        <html>
                        <head>
                        <title>Page Title</title>
                        </head>
                        <body>

                        <h1>My First Heading</h1>
                        <p>My first paragraph.</p>

                        </body>
                        </html>
                        ";
        for (int i = 1; i <= 30; i++)
        {
            blogPostList.Add(BlogPost.Create($"{i}", $"test{i}",content,"https://images.agi.it/pictures/agi/agi/2021/11/15/143214429-dd3e4ea6-2844-48b3-a099-a917bbb27d52.jpg",author,category,new List<Tag>(){tag,tag2}));
        }
        context.Categories.Add(category);
        context.Authors.Add(author);
        context.Tags.AddRange(tag,tag2);
        context.BlogPosts.AddRange(blogPostList);
        await context.SaveChangesAsync();
        var client = TestClient.CreateHttpClient(_testOutputHelper,connectionString);
        client.DefaultRequestHeaders.Add("X-USER", "user");
        //ATTEMPT
        var response = await client.GetAsync(request.Url);
        //VERIFY
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var model = await response.Content.ReadFromJsonAsync<BlogPostPaginationResponse>();
        await Verifier.Verify(model,TestClient.GetVerifySettings());
        await TestClient.CheckDatabaseAndRemoveIt(context);
    }
    [Fact]
    public async Task GetBlogPosts_WithNotExistingName_ShouldReturnNoContent()
    {
        //SETUP
        var request = new
        {
            Url = "/api/BlogPosts?",
        };
        await using var context = TestClient.GetDbContext(nameof(this.GetBlogPosts_WithNotExistingName_ShouldReturnNoContent),out var connectionString);
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