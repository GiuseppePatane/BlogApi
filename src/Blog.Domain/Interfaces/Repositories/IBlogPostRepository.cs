namespace Blog.Domain.Interfaces.Repositories;

public interface IBlogPostRepository:IGenericRepository
{
    Task<bool> GetByTitle(string title);
}