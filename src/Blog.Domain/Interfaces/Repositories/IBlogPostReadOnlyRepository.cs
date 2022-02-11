using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces.Repositories;

public interface IBlogPostReadOnlyRepository
{
    Task<BlogPostPaginationResponse> GetBlogPostsPaginate(int page, int perPage,string blogTile, string category, List<string>tags);
}