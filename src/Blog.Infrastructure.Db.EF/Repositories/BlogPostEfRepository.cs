using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Db.EF.Repositories;

public class BlogPostEfRepository : EfRepository, IBlogPostRepository
{
    public BlogPostEfRepository(BlogDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> GetByTitleAsync(string title)
    {
        return DbContext.BlogPosts.AnyAsync(x => x.Title != null && x.Title.Trim().Equals(title.Trim()));
    }

    public Task<BlogPost?> GetWithTagsAsync(string id)
    {
        return DbContext.BlogPosts.Include(x=>x.TagXBlogPosts).FirstOrDefaultAsync(x => x.Id == id);
    }
}