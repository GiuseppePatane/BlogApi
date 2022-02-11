using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.Interfaces.Services;

namespace Blog.Core;

public class BlogPostService : IBlogPostService
{
    private readonly IIdGenerator _idGenerator;
    private readonly IBlogPostRepository _repository;

    public BlogPostService(IBlogPostRepository repository, IIdGenerator idGenerator)
    {
        _repository = repository;
        _idGenerator = idGenerator;
    }

    public async Task<CreateResponse> Create(CreateBlogPostRequest request)
    {
        var exist = await _repository.GetByTitleAsync(request.Title);
        if (exist)
            throw new DomainException(BlogPost.AlreadyExistError);
        var author = await GetAuthor(request.AuthorId);
        var category = await GetCategory(request.CategoryId);
        var tags = await GetTags(request.Tags);
        var blogPost = BlogPost.Create(_idGenerator.GenerateId(), request.Title, request.Content, request.Image,
            author, category, tags);
        await _repository.AddAsync(blogPost);
        return new CreateResponse(blogPost.Id);
    }

    public async Task Update(string id, UpdateBlogPostRequest request)
    {
        var blogPost = await _repository.GetByIdAsync<BlogPost>(id);
        if (blogPost == null) throw new DomainException(BlogPost.NotFoundError);
        blogPost.Update(request.Title, request.Content, request.Image);
        await _repository.UpdateAsync(blogPost);
    }

    public async Task UpdateCategory(string id, string categoryId)
    {
        var blogPost = await _repository.GetByIdAsync<BlogPost>(id);
        if (blogPost == null) throw new DomainException(BlogPost.NotFoundError);
        var category = await _repository.GetByIdAsync<Category>(categoryId);
        if (category == null) throw new DomainException(Category.NotFoundError);
        blogPost.UpdateCategory(category);
        await _repository.UpdateAsync(blogPost);
    }

    public async Task AssociateTag(string id, string tagId)
    {
        var blogPost = await _repository.GetWithTagsAsync(id);
        if (blogPost == null) throw new DomainException(BlogPost.NotFoundError);
        var tag = await _repository.GetByIdAsync<Tag>(tagId);
        if (tag == null) throw new DomainException(Tag.NotFoundError);
        blogPost.AssociateTag(tag);
        await _repository.UpdateAsync(blogPost);
    }


    private async Task<List<Tag>> GetTags(List<string> requestTags)
    {
        var tags = await _repository.ListAsync<Tag>(x => requestTags.Contains(x.Id));
        if (tags == null || !tags.Any())
            throw new DomainException(Tag.NotFoundError);

        return tags;
    }

    private async Task<Author> GetAuthor(string authorId)
    {
        var author = await _repository.GetByIdAsync<Author>(authorId);
        if (author == null)
            throw new DomainException(Author.NotFoundError);
        return author;
    }

    private async Task<Category> GetCategory(string categoryId)
    {
        var category = await _repository.GetByIdAsync<Category>(categoryId);
        if (category == null)
            throw new DomainException(Category.NotFoundError);
        return category;
    }
}