using Blog.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Db.EF.Repositories;

public class CategoryEfRepository : EfRepository, ICategoryRepository
{
    public CategoryEfRepository(BlogDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> GetByNameAsync(string title)
    {
        return DbContext.Categories.AnyAsync(x => x.Name.Trim().Equals(title.Trim()));
    }
}