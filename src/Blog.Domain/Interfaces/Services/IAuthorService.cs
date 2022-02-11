using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces.Services;

public interface IAuthorService
{
    public Task<CreateResponse> Create(CreateAuthorRequest request);
}