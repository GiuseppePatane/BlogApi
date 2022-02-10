using Blog.Domain.DTOs;
using Blog.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
public class BlogPostController:Controller
{
    private readonly IBlogPostService _blogPostService;

    public BlogPostController(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    /// <summary>
    /// Create a new blog post 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/BlogPost")]
    public async Task<IActionResult> CreateBlogPost(CreateBlogPostRequest request)
    {
        var response = await _blogPostService.Create(request);
        return Ok(response);
    }

    /// <summary>
    /// update blog post  information
    /// </summary>
    /// <param name="id"> blog post id </param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch]
    [Route("api/BlogPost/{id}")]
    public async Task<IActionResult> UpdateBlogPost(string id,UpdateBlogPostRequest request)
    {
         await _blogPostService.Update(id,request);
        return Ok(new ErrorResponse());
    }
    
    /// <summary>
    /// Update blog Post category
    /// </summary>
    /// <param name="id"> blog post id </param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch]
    [Route("api/BlogPost/{id}/Category/{categoryId}")]
    public async Task<IActionResult> UpdateCategory(string id,string categoryId)
    {
        await _blogPostService.UpdateCategory(id,categoryId);
        return Ok(new ErrorResponse());
    }
    
    /// <summary>
    /// Update blog Post category
    /// </summary>
    /// <param name="id"> blog post id </param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch]
    [Route("api/BlogPost/{id}/Tags/{tagId}")]
    public async Task<IActionResult> AddNewTag(string id,string tagId)
    {
        await _blogPostService.AssociateTag(id,tagId);
        return Ok(new ErrorResponse());
    }
}