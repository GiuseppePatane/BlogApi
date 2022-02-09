using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces;

public interface IAuthorService 
{
    public Task<CreateResponse> Create(CreateAuthorRequest request);
}