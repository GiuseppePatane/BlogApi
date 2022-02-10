using System.Linq.Expressions;
using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces.Repositories;

public interface IGenericRepository
{
    Task<T?> GetByIdAsync<T>(string id) where T : BaseEntity;

    Task<List<T>> ListAsync<T>() where T : BaseEntity;
    Task<List<T>> ListAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity;

    Task<T> AddAsync<T>(T entity) where T : BaseEntity;

    Task UpdateAsync<T>(T entity) where T : BaseEntity;

    Task DeleteAsync<T>(T entity) where T : BaseEntity;
}