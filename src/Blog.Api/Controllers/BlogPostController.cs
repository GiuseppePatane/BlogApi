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
}