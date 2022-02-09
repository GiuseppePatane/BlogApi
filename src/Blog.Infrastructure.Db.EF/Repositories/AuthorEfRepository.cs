using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Db.EF.Repositories;

public class AuthorEfRepository : EfRepository, IAuthorRepository
{
    public AuthorEfRepository(BlogDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> GetByNameAsync(string title)
    {
        return DbContext.Authors.AnyAsync(x => x.Name.Trim().Equals(title.Trim()));
    }
}