using Blog.Domain.Interfaces;
using Blog.Infrastructure.Db.EF.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure;

public static class StartupSetupRepository
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IRepository, EfRepository>();
        services.AddTransient<IAuthorRepository, AuthorEfRepository>();
        services.AddTransient<ICategoryRepository, CategoryEfRepository>();
        services.AddTransient<ITagRepository, TagEfRepository>();
    }
}