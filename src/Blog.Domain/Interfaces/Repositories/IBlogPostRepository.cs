using Blog.Domain.DTOs;
using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces.Repositories;

public interface IBlogPostRepository:IGenericRepository
{
    Task<bool> GetByTitleAsync(string title);
    Task<BlogPost?> GetWithTagsAsync(string id);
    Task<BlogPostPaginationResponse?> GetBlogPostsPaginate(int page, int perPage,string blogTile, string category, List<string>tags);
    Task<BlogPostResponse?> GetBlogPosts(string id);
}