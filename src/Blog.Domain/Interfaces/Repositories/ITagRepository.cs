namespace Blog.Domain.Interfaces.Repositories;

public interface ITagRepository : IGenericRepository
{
    Task<bool> GetByNameAsync(string title);
}