using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.Interfaces.Services;

namespace Blog.Core;

public class TagService : ITagService
{
    private readonly ITagRepository _repository;
    private readonly IIdGenerator _idGenerator;

    public TagService(ITagRepository repository, IIdGenerator idGenerator)
    {
        _repository = repository;
        _idGenerator = idGenerator;
    }

    public async Task<CreateResponse> Create(CreateTagRequest request)
    {
        var exist = await _repository.GetByNameAsync(request.Name);
        if (exist)
            throw new DomainException(Tag.AlreadyExistError);

        var author = Tag.Create(_idGenerator.GenerateId(), request.Name);
        await _repository.AddAsync(author);
        return new CreateResponse(author?.Id);
    }

    public Task<TagPaginationResponse?> GetTags(int page, int perPage, string name)
    {
        return  _repository.GetTagPaginate(page,perPage,name);
    }
}