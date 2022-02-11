using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces.Repositories;

public interface ICategoryRepository:IGenericRepository
{
    Task<bool> GetByNameAsync(string title);
    Task<CategoryPaginationResponse?> GetCategoriesPaginate(int page, int perPage, string name);
}