using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces.Services;

public interface ICategoryService 
{
    public Task<CreateResponse> Create(CreateCategoryRequest request);
}