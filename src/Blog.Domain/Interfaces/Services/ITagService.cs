using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces.Services;

public interface ITagService 
{
    public Task<CreateResponse> Create(CreateTagRequest request);
}