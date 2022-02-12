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
    [XUserAuthorizationFilter(new []{AuthConst.UserRole,AuthConst.AdminRole})]
    [ProducesResponseType(typeof(CreateResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse),400)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    public async Task<IActionResult> CreateAuthor(CreateAuthorRequest request)
    {
        var response = await _authorService.Create(request);
        return Ok(response);
    }
    
    
    /// <summary>
    /// Get a paginate list of author
    /// </summary>
    /// <param name="page">requested page </param>
    /// <param name="perPage"> items per page </param>
    /// <param name="name"> author name you want to search for</param>
    /// <returns></returns>
    [HttpGet]
    [Route("api/Authors")]
    [XUserAuthorizationFilter(new []{AuthConst.UserRole,AuthConst.AdminRole})]
    [ProducesResponseType(typeof(AuthorPaginationResponse), 200)]
    [ProducesResponseType( 204)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    public async Task<IActionResult> GetAuthor(int page, int perPage, string name)
    {
        if (page == 0) page = 1;
        if (perPage == 0) perPage = 10;
        var result = await _authorService.GetAuthors(page, perPage, name);
        return Ok(result);
    }
}