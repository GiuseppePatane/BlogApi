using Blog.Domain.DTOs;

namespace Blog.Domain.Interfaces;

public interface ITagService 
{
    public Task<CreateResponse> Create(CreateTagRequest request);
}