using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces.Repositories;

public interface IAuthorRepository:IGenericRepository
{
    Task<bool> GetByNameAsync(string title);
    Task<AuthorPaginationResponse?> GetAuthorsPaginate(int page, int perPage, string name);
}