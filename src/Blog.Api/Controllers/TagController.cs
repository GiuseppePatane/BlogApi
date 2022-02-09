using Blog.Domain.DTOs;
using Blog.Domain.Interfaces;
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
    public async Task<IActionResult> CreateTag(CreateTagRequest request)
    {
        var response = await _tagService.Create(request);
        return Ok(response);
    }
}