using Blog.Api.Auth;
using Blog.Api.Filters;
using Blog.Domain.DTOs;
using Blog.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
public class AuthorController : Controller
{
    
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    /// <summary>
    /// Create a new author 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/Author")]
    [XUserAuthorizationFilter(AuthConst.UserRole)]
    public async Task<IActionResult> CreateAuthor(CreateAuthorRequest request)
    {
        var response = await _authorService.Create(request);
        return Ok(response);
    }
}