namespace Blog.Domain.Interfaces.Repositories;

public interface IAuthorRepository:IGenericRepository
{
    Task<bool> GetByNameAsync(string title);
}