using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.Interfaces.Services;

namespace Blog.Core;

public class AuthorService : IAuthorService
{
    private readonly IIdGenerator _idGenerator;
    private readonly IAuthorRepository _repository;

    public AuthorService(IAuthorRepository repository, IIdGenerator idGenerator)
    {
        _repository = repository;
        _idGenerator = idGenerator;
    }

    public async Task<CreateResponse> Create(CreateAuthorRequest request)
    {
        var exist = await _repository.GetByNameAsync(request.Name);
        if (exist)
            throw new DomainException(Author.AlreadyExistError);

        var author = Author.Create(_idGenerator.GenerateId(), request.Name);
        await _repository.AddAsync(author);
        return new CreateResponse(author?.Id);
    }
}