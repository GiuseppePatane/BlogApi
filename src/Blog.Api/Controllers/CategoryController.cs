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
    /// Create a new category 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>+
    [ProducesResponseType(typeof(CreateResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse),400)]
    [ProducesResponseType(typeof(ErrorResponse),500)]
    [HttpPost]
    [Route("api/Category")]
    [XUserAuthorizationFilter(new []{AuthConst.UserRole,AuthConst.AdminRole})]
    public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
    {
        var response = await _categoryService.Create(request);
        return Ok(response);
    }
    
    /// <summary>
    /// Get a paginate list of categories
    /// </summary>
    /// <param name="page">the request page.Default 1</param>
    /// <param name="perPage"> number of element per page. Default 10</param>
    /// <param name="name"> category name you want to search for</param>
    /// <returns></returns>
    [HttpGet]
    [Route("api/Categories")]
    [XUserAuthorizationFilter(new []{AuthConst.UserRole,AuthConst.AdminRole})]
    public async Task<IActionResult> GetCategories(int page, int perPage, string name)
    {
        if (page == 0) page = 1;
        if (perPage == 0) perPage = 10;
        var result = await _categoryService.GetCategories(page, perPage, name);
        return Ok(result);
    }
}