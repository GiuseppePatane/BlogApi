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
    /// Create a new author 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/Tag")]
    [XUserAuthorizationFilter(AuthConst.UserRole)]
    public async Task<IActionResult> CreateTag(CreateTagRequest request)
    {
        var response = await _tagService.Create(request);
        return Ok(response);
    }
    
    [HttpGet]
    [Route("api/Tags")]
    [XUserAuthorizationFilter(AuthConst.UserRole)]
    public async Task<IActionResult> GetTags(int page, int perPage, string name)
    {
        if (page == 0) page = 1;
        if (perPage == 0) perPage = 10;
        var result = await _tagService.GetTags(page, perPage, name);
        return Ok(result);
    }
}