using Blog.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Db.EF.Repositories;

public class BlogPostEfRepository : EfRepository, IBlogPostRepository
{
    public BlogPostEfRepository(BlogDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> GetByTitle(string title)
    {
        return DbContext.BlogPosts.AnyAsync(x => x.Title != null && x.Title.Trim().Equals(title.Trim()));
    }
}