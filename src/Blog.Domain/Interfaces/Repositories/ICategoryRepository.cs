namespace Blog.Domain.Interfaces.Repositories;

public interface ICategoryRepository : IGenericRepository
{
    Task<bool> GetByNameAsync(string title);
}