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
        var loggerFactory =services.GetService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(nameof(WebHostExtensions));
        var context = services.GetService<BlogDbContext>();
         logger.LogInformation($"connectionString:{context.Database.GetConnectionString()}");
        if (context != null && context.Database.CanConnect())
        {
            logger.LogInformation("Delete database!!!!");
            context.Database.EnsureDeleted();
        }
        logger.LogInformation("Start db migration");
        context?.Database.Migrate();
        logger.LogInformation("db migration finished");

        return host;
    }
}