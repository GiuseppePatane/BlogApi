using Blog.Infrastructure.Db.EF;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Extensions;

public static class WebHostExtensions
{
    public static IHost SeedData(this IHost host, string[] args)
    {
        if (!args.Contains("seed")) return host;
        
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetService<BlogDbContext>();

        if (context != null && context.Database.CanConnect())
        {
            context.Database.EnsureDeleted();
        }
        context?.Database.Migrate();

        return host;
    }
}