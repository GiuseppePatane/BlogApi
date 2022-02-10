using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.Interfaces.Services;

namespace Blog.Core;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IIdGenerator _idGenerator;

    public CategoryService(ICategoryRepository repository, IIdGenerator idGenerator)
    {
        _repository = repository;
        _idGenerator = idGenerator;
    }

    public async Task<CreateResponse> Create(CreateCategoryRequest request)
    {
        var exist = await _repository.GetByNameAsync(request.Name);
        if (exist)
            throw new DomainException(Category.AlreadyExistError);
        
        var author = Category.Create(_idGenerator.GenerateId(), request.Name);
        await _repository.AddAsync(author);
        return new CreateResponse(author?.Id);
    }
}