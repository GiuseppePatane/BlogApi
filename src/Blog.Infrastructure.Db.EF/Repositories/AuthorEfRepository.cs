using Blog.Domain.DTOs;
using Blog.Domain.Interfaces.Repositories;
using Blog.Infrastructure.Db.EF.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Db.EF.Repositories;

public class AuthorEfRepository : EfRepository, IAuthorRepository
{
    public AuthorEfRepository(BlogDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> GetByNameAsync(string title)
    {
        return DbContext.Authors.AnyAsync(x => x.Name != null && x.Name.Trim().Equals(title.Trim()));
    }

    public async Task<AuthorPaginationResponse?> GetAuthorsPaginate(int page, int perPage, string name)
    {
        var queryResult=  DbContext.Authors
            .Where(x=>string.IsNullOrWhiteSpace(name)|| x.Name.Contains(name))
            .OrderBy(x=>x.CreationDateUtc)
            .Select(x => new AuthorResponse()
            {
                Id = x.Id ?? string.Empty,
                Name = x.Name ?? string.Empty,
            });

        var skip = PaginationHelper.Skip(page, perPage);
        var result= new AuthorPaginationResponse()
        {
            Items = await PaginationHelper.GetItemsAsync(skip,perPage,queryResult),
            TotalHits = await queryResult.CountAsync(),
            Size = perPage,
            TotalPages = await PaginationHelper.CountAsync(perPage, queryResult)
        };
       return result.Items.Any() ? result : null;
    }
}