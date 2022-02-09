using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces;

public interface ICategoryService 
{
    public Task<CreateResponse> Create(CreateCategoryRequest request);
}