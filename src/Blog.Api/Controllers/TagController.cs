using Blog.Api.Auth;
using Blog.Api.Filters;
using Blog.Domain.DTOs;
using Blog.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
public class TagController : Controller
{
    
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    /// <summary>
    /// Create a new tag 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/Tag")]
    [XUserAuthorizationFilter(new []{AuthConst.UserRole,AuthConst.AdminRole})]
    [ProducesResponseType(typeof(CreateResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse),400)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    public async Task<IActionResult> CreateTag(CreateTagRequest request)
    {
        var response = await _tagService.Create(request);
        return Ok(response);
    }
    
    /// <summary>
    /// Get a paginate list of tags
    /// </summary>
    /// <param name="page">requested page </param>
    /// <param name="perPage"> items per page </param>
    /// <param name="name"> tag name you want to search for</param>
    /// <returns></returns>
    [HttpGet]
    [Route("api/Tags")]
    [XUserAuthorizationFilter(new []{AuthConst.UserRole,AuthConst.AdminRole})]
    [ProducesResponseType(typeof(TagPaginationResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    public async Task<IActionResult> GetTags(int page, int perPage, string name)
    {
        if (page == 0) page = 1;
        if (perPage == 0) perPage = 10;
        var result = await _tagService.GetTags(page, perPage, name);
        return Ok(result);
    }
}