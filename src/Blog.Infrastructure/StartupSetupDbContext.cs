using Blog.Domain.Interfaces;
using Blog.Infrastructure.Db.EF;
using Blog.Infrastructure.Db.SqlKata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure;

public static class StartupSetupDbContext
{
    public static void AddDbContextWithPostgresql(this IServiceCollection services, string connectionString)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddTransient<IDatabaseConnectionFactory>(x =>
            new NpgsqlConnectionFactory(connectionString));
        services.AddDbContext<BlogDbContext>(options =>
            options.
                UseNpgsql(connectionString)
        );
    }
}

