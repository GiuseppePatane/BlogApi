using Blog.Api.Filters;
using Blog.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
public class BlogPostController:Controller
{

    /// <summary>
    /// Create a new blog post 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/BlogPost")]
    public IActionResult CreateBlogPost(CreateBlogPostRequest request)
    {
        return Ok();
    }
}