using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Db.EF.Helpers;

public static class PaginationHelper
{
    public static async Task<int> CountAsync<T>(int perPage, IQueryable<T> result)
    {
        var count= (await result.CountAsync() + perPage - 1) / perPage;
        return count;
    }
    public static async Task<List<T>> GetItemsAsync<T>(int skip ,int perPage, IQueryable<T> result)
    {
        return await result.Skip(skip).Take(perPage).ToListAsync();
    }

    public static int Skip(int page, int perPage ) => (page - 1) * perPage;
   
}