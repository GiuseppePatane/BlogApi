using System.Collections;
using Blog.Infrastructure.Db.EF;
using Blog.Infrastructure.Db.EF.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Extensions;

public static class WebHostExtensions
{
    public static IHost SeedData(this IHost host, string[] args)
    {
        var seed  = Environment.GetEnvironmentVariable("SEED");
        if (string.IsNullOrEmpty(seed)) return host;
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory =services.GetService<ILoggerFactory>();
        var logger = loggerFactory?.CreateLogger(nameof(WebHostExtensions));
        var context = services.GetService<BlogDbContext>();
        if (context != null && !context.Database.CanConnect()) 
        {
             logger.LogWarning("db not found, starting migration");
             context?.Database.Migrate();
             logger.LogWarning("migration finished...Init Test data");
             context?.InitTestData(logger);
             logger.LogWarning("init test data finished ");
             
        }
        else
        {
             context.InitTestData(logger);
        }
        return host;
    }
}