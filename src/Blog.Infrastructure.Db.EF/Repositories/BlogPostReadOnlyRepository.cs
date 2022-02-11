using Blog.Domain.DTOs;
using Blog.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Db.EF.Repositories;

public class BlogPostReadOnlyEfRepository : EfRepository, IBlogPostReadOnlyRepository
{
    public BlogPostReadOnlyEfRepository(BlogDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<BlogPostPaginationResponse> GetBlogPostsPaginate(int page, int perPage,string blogTile, string category, List<string>tags)
    {
        var skip = (page - 1) * perPage;
        var result=  DbContext.BlogPosts
            .Include(x => x.Author)
            .Include(x => x.Category)
            .Include(x => x.TagXBlogPosts).ThenInclude(x => x.Tag)
            .Where(x=>string.IsNullOrWhiteSpace(blogTile)|| x.Title.Contains(blogTile))
            .Where(x=>string.IsNullOrWhiteSpace(category)|| x.Category.Name==category)
            .Where(x=> !tags.Any() || x.TagXBlogPosts.Any(x=> tags.Contains(x.Tag.Name)) )
            .OrderBy(x=>x.CreationDateUtc)
            .Select(x => new BlogPostResponse()
            {
                BlogPostId = x.Id ?? string.Empty,
                Title = x.Title ?? string.Empty,
                Content = x.Content ?? string.Empty,
                Image = x.Image ?? string.Empty,
                AuthorName = x.Author.Name ?? string.Empty,
                CategoryName = x.Category.Name ?? string.Empty,
                Tags = x.TagXBlogPosts.Select(x=>x.Tag.Name).ToList()
            });

        
        return new BlogPostPaginationResponse()
        {
            Items = await result.Skip(skip).Take(perPage).ToListAsync(),
            TotalHits = await result.CountAsync(),
            Size = perPage,
            TotalPages = (await result.CountAsync() + perPage - 1) / perPage
        };
    }
}