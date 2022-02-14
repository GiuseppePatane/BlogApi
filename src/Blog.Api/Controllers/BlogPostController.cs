using Blog.Api.Auth;
using Blog.Api.Filters;
using Blog.Domain.DTOs;
using Blog.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
public class BlogPostController : Controller
{
    private readonly IBlogPostService _blogPostService;

    public BlogPostController(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    /// <summary>
    /// Create a new blog post.
    /// The url image must correspond to a valid path of a hypothetical cnd.ex http://mycdn.com/image.jpg
    /// Max title lenght 255
    /// Max content lenght 1024.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/BlogPost")]
    [XUserAuthorizationFilter(new []{AuthConst.UserRole,AuthConst.AdminRole})]
    [ProducesResponseType(typeof(CreateResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse),400)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    public async Task<IActionResult> CreateBlogPost(CreateBlogPostRequest request)
    {
        var response = await _blogPostService.Create(request);
        return Ok(response);
    }

    /// <summary>
    /// Update blog post  information
    /// The url image must correspond to a valid path of a hypothetical cnd.ex http://mycdn.com/image.jpg
    /// Max title lenght 255
    /// Max content lenght 1024
    /// </summary>
    /// <param name="id"> blog post id </param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch]
    [Route("api/BlogPost/{id}")]
    [XUserAuthorizationFilter(new []{AuthConst.UserRole,AuthConst.AdminRole})]
    [ProducesResponseType(typeof(ErrorResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse),400)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    public async Task<IActionResult> UpdateBlogPost(string id, UpdateBlogPostRequest request)
    {
        await _blogPostService.Update(id, request);
        return Ok(new ErrorResponse());
    }

    /// <summary>
    /// Update blog post category id 
    /// </summary>
    /// <param name="id"> blog post id </param>
    /// <param name="categoryId"> the new category id</param>
    /// <returns></returns>
    [HttpPatch]
    [Route("api/BlogPost/{id}/Category/{categoryId}")]
    [XUserAuthorizationFilter(new []{AuthConst.UserRole,AuthConst.AdminRole})]
    [ProducesResponseType(typeof(ErrorResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse),400)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    public async Task<IActionResult> UpdateCategory(string id, string categoryId)
    {
        await _blogPostService.UpdateCategory(id, categoryId);
        return Ok(new ErrorResponse());
    }


    /// <summary>
    ///  Add a new tag to a blog post
    /// </summary>
    /// <param name="id">blog post id </param>
    /// <param name="tagId"> the new tag id</param>
    /// <returns></returns>
    [HttpPatch]
    [Route("api/BlogPost/{id}/Tags/{tagId}")]
    [XUserAuthorizationFilter(new []{AuthConst.UserRole,AuthConst.AdminRole})]
    [ProducesResponseType(typeof(ErrorResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse),400)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    public async Task<IActionResult> AddNewTag(string id, string tagId)
    {
        await _blogPostService.AssociateTag(id, tagId);
        return Ok(new ErrorResponse());
    }


    /// <summary>
    /// Delete a blog post 
    /// </summary>
    /// <param name="id">the blogPostId to delete </param>
    /// <returns></returns>
    [HttpDelete]
    [Route("api/BlogPost/{id}")]
    [XUserAuthorizationFilter(new []{AuthConst.AdminRole})]
    [ProducesResponseType(typeof(ErrorResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse),400)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    public async Task<IActionResult> DeleteBlogPost(string id)
    {
        await _blogPostService.DeleteBlogPost(id);
        return Ok(new ErrorResponse());
    }

    /// <summary>
    /// Get paginate list of blog post.
    /// Content will be shown abbreviation followed by three dots if the length is greater than 150 characters.
    /// </summary>
    /// <param name="page">the request page.Default 1</param>
    /// <param name="perPage"> number of element per page. Default 10</param>
    /// <param name="title"> blog title thant you want search</param>
    /// <param name="category">blog category </param>
    /// <param name="tags"> search for the list associated tags </param>
    /// <returns></returns>
    [HttpGet]
    [Route("api/BlogPosts")]
    [XUserAuthorizationFilter(new []{AuthConst.UserRole,AuthConst.AdminRole})]
    [ProducesResponseType(typeof(BlogPostPaginationResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    public async Task<IActionResult> GetBlogPosts(int page, int perPage, string title, string category,
        [FromQuery] List<string> tags)
    {
        if (page == 0) page = 1;
        if (perPage == 0) perPage = 10;
        var result = await _blogPostService.GetBlotPosts(page, perPage, title, category, tags);
        return Ok(result);
    }
    
    /// <summary>
    /// Get a blogPost by id 
    /// </summary>
    /// <param name="id">the blog post id </param>
    /// <returns></returns>
    [HttpGet]
    [Route("api/BlogPost/{id}")]
    [XUserAuthorizationFilter(new []{AuthConst.UserRole,AuthConst.AdminRole})]
    [ProducesResponseType(typeof(BlogPostResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    public async Task<IActionResult> GetBlogPost(string id)
    {
        var result = await _blogPostService.GetBlotPost(id);
        return Ok(result);
    }
}