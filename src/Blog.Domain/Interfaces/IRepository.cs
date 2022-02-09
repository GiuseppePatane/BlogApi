using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces;

public interface IRepository
{
    Task<T?> GetByIdAsync<T>(string id) where T : BaseEntity;

    Task<List<T>> ListAsync<T>() where T : BaseEntity;

    Task<T> AddAsync<T>(T entity) where T : BaseEntity;

    Task UpdateAsync<T>(T entity) where T : BaseEntity;

    Task DeleteAsync<T>(T entity) where T : BaseEntity;
}