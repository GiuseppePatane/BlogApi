using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces.Repositories;

public interface IBlogPostRepository:IGenericRepository
{
    Task<bool> GetByTitleAsync(string title);
    Task<BlogPost?> GetWithTagsAsync(string id);
}