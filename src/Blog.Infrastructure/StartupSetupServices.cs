using Blog.Core;
using Blog.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure;

public static class StartupSetupServices
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthorService, AuthorService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ITagService, TagService>();
        services.AddTransient<IIdGenerator, IdGuidGenerator>();
       

    }
}