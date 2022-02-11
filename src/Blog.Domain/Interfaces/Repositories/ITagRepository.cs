using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces.Repositories;

public interface ITagRepository:IGenericRepository
{
    Task<bool> GetByNameAsync(string title);
    Task<TagPaginationResponse?> GetTagPaginate(int page, int perPage, string name);
}