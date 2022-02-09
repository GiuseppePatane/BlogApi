using Blog.Api.Filters;
using Blog.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
public class AuthorController : Controller
{
    /// <summary>
    /// Create a new author 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/Author")]
    public IActionResult CreateAuthor(CreateAuthorRequest request)
    {
        return Ok();
    }
}


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