using Blog.Domain.Interfaces.Repositories;
using Blog.Infrastructure.Db.EF.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure;

public static class StartupSetupRepository
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IGenericRepository, EfRepository>();
        services.AddTransient<IAuthorRepository, AuthorEfRepository>();
        services.AddTransient<ICategoryRepository, CategoryEfRepository>();
        services.AddTransient<ITagRepository, TagEfRepository>();
        services.AddTransient<IBlogPostRepository, BlogPostEfRepository>();
    }
}