using Blog.Domain.DTOs;
using Blog.Domain.Interfaces.Repositories;
using Blog.Infrastructure.Db.EF.Helpers;
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

    public async Task<TagPaginationResponse?> GetTagPaginate(int page, int perPage, string name)
    {
        var queryResult=  DbContext.Tags
            .Where(x=>string.IsNullOrWhiteSpace(name)|| x.Name.Contains(name))
            .OrderBy(x=>x.CreationDateUtc)
            .Select(x => new TagResponse()
            {
                Id = x.Id ?? string.Empty,
                Name = x.Name ?? string.Empty,
            });

        var skip = PaginationHelper.Skip(page, perPage);
        var result= new TagPaginationResponse()
        {
            Items = await PaginationHelper.GetItemsAsync(skip,perPage,queryResult),
            Page=page,
            TotalHits = await queryResult.CountAsync(),
            Size = perPage,
            TotalPages = await PaginationHelper.CountAsync(perPage, queryResult)
        };
        return result.Items.Any() ? result : null;
    }
}