using System.Linq.Expressions;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Db.EF.Repositories;

public class EfRepository : IGenericRepository
{
    protected readonly BlogDbContext DbContext;

    public EfRepository(BlogDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public Task<T?> GetByIdAsync<T>(string id) where T : BaseEntity
    {
        return DbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
    }

    public Task<List<T>> ListAsync<T>() where T : BaseEntity
    {
        return DbContext.Set<T>().ToListAsync();
    }

    public Task<List<T>> ListAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
    {
        return DbContext.Set<T>().Where(predicate).ToListAsync();
    }


    public async Task<T> AddAsync<T>(T entity) where T : BaseEntity
    {
        await DbContext.Set<T>().AddAsync(entity);
        await DbContext.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync<T>(T entity) where T : BaseEntity
    {
        DbContext.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync<T>(T entity) where T : BaseEntity
    {
        DbContext.Set<T>().Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    public T? GetById<T>(string id) where T : BaseEntity
    {
        return DbContext.Set<T>().FirstOrDefault(e => e.Id == id);
    }
}