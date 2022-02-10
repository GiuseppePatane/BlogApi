using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces.Services;

public interface IBlogPostService
{
    public Task<CreateResponse> Create(CreateBlogPostRequest request);
}