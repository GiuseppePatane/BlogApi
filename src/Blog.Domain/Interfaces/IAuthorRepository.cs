using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces;

public interface IAuthorRepository:IRepository
{
    Task<bool> GetByNameAsync(string title);
}
public interface ICategoryRepository:IRepository
{
    Task<bool> GetByNameAsync(string title);
}

public interface ITagRepository:IRepository
{
    Task<bool> GetByNameAsync(string title);
}