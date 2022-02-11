using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces.Services;

public interface IAuthorService 
{
    public Task<CreateResponse> Create(CreateAuthorRequest request);
    Task<AuthorPaginationResponse?> GetAuthors(int page, int perPage, string name);
}