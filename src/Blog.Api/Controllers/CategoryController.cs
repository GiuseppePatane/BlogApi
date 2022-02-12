using Blog.Api.Auth;
using Blog.Api.Filters;
using Blog.Domain.DTOs;
using Blog.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
public class CategoryController : Controller
{
    
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Create a new author 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/Category")]
    [XUserAuthorizationFilter(AuthConst.UserRole)]
    public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
    {
        var response = await _categoryService.Create(request);
        return Ok(response);
    }
    
    [HttpGet]
    [Route("api/Categories")]
    [XUserAuthorizationFilter(AuthConst.UserRole)]
    public async Task<IActionResult> GetCategories(int page, int perPage, string name)
    {
        if (page == 0) page = 1;
        if (perPage == 0) perPage = 10;
        var result = await _categoryService.GetCategories(page, perPage, name);
        return Ok(result);
    }
}