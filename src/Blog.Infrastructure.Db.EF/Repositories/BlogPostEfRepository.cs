using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using Blog.Domain.Extensions;
using Blog.Domain.Interfaces.Repositories;
using Blog.Infrastructure.Db.EF.Helpers;
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

    public async Task<BlogPostPaginationResponse?> GetBlogPostsPaginate(int page, int perPage,string blogTile, string category, List<string>tags)
    {
      
        var queryResult=  DbContext.BlogPosts
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
                Content = x.Content.TrimAndTruncateHtml(150,"...")??string.Empty,
                ImageUrl = x.Image ?? string.Empty,
                AuthorName = x.Author.Name ?? string.Empty,
                CategoryName = x.Category.Name ?? string.Empty,
                Tags = x.TagXBlogPosts.Select(x=>x.Tag.Name).ToList()
            });

        var skip = PaginationHelper.Skip(page, perPage);
        var result= new BlogPostPaginationResponse()
        {
            Items = await PaginationHelper.GetItemsAsync(skip,perPage,queryResult),
            Page=page,
            TotalHits = await queryResult.CountAsync(),
            Size = perPage,
            TotalPages = await PaginationHelper.CountAsync(perPage, queryResult)
        };
        return result.Items.Any() ? result : null;
    }

    public async Task<BlogPostResponse?> GetBlogPosts(string id)
    {
        var queryResult = await DbContext.BlogPosts
            .Include(x => x.Author)
            .Include(x => x.Category)
            .Include(x => x.TagXBlogPosts).ThenInclude(x => x.Tag)
            .FirstOrDefaultAsync(x => x.Id == id);

        return queryResult==null  
            ? null
            : new BlogPostResponse()
        {
            BlogPostId = queryResult?.Id ?? string.Empty,
            Title = queryResult?.Title ?? string.Empty,
            Content = queryResult?.Content ?? string.Empty,
            ImageUrl = queryResult?.Image ?? string.Empty,
            AuthorName = queryResult?.Author.Name ?? string.Empty,
            CategoryName = queryResult?.Category.Name ?? string.Empty,
            Tags = queryResult?.TagXBlogPosts.Select(x => x.Tag.Name).ToList()
        };

    }
}

