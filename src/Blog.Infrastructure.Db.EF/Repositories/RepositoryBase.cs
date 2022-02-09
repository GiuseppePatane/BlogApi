using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Db.EF.Repositories;

public class EfRepository : IRepository
{
    protected readonly BlogDbContext DbContext;

    public EfRepository(BlogDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public Task<T?> GetByIdAsync<T>(string id) where T : BaseEntity
    {
        return DbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
    }

    public Task<List<T>> ListAsync<T>() where T : BaseEntity
    {
        return DbContext.Set<T>().ToListAsync();
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
        return DbContext.Set<T>().SingleOrDefault(e => e.Id == id);
    }
    
}