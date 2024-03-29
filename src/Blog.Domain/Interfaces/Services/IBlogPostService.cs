using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces.Services;

public interface IBlogPostService
{
    public Task<CreateResponse> Create(CreateBlogPostRequest request);
    Task Update(string id,UpdateBlogPostRequest request);
    Task UpdateCategory(string id, string categoryId);
    Task AssociateTag(string id, string tagId);
    Task DeleteBlogPost(string id);
    Task<BlogPostPaginationResponse?> GetBlotPosts(int page, int perPage, string title, string category, List<string> tags);
    Task<BlogPostResponse?> GetBlotPost(string id);
}