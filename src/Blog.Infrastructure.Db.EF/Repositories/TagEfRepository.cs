using Blog.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Db.EF.Repositories;

public class TagEfRepository : EfRepository, ITagRepository
{
    public TagEfRepository(BlogDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> GetByNameAsync(string title)
    {
        return DbContext.Tags.AnyAsync(x => x.Name.Trim().Equals(title.Trim()));
    }
}