using System.Threading.Tasks;
using Blog.Infrastructure.Db.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TestSupport.Helpers;

namespace Blog.IntegrationTests;

public class IntegrationTestBase
{
    protected DbContextOptions<BlogDbContext> GenDbContextOptions()
    {
        var config = AppSettings.GetConfiguration();
        var npgsqlBuilder = new NpgsqlConnectionStringBuilder(
            config.GetConnectionString(AppSettings.PostgreSqlConnectionString)).Database;
        var connectionString = this.GetUniquePostgreSqlConnectionString();
        var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.EnableSensitiveDataLogging();
        return optionsBuilder.Options;
    }

    protected async Task CheckDatabaseAndRemoveIt(BlogDbContext dbContext)
    {
        if (await dbContext.Database.CanConnectAsync().ConfigureAwait(false))
        {
            await dbContext.Database.EnsureDeletedAsync().ConfigureAwait(false);
        }
    }
    protected   async Task PrepareDatabase(BlogDbContext appDbContext)
    {
        await CheckDatabaseAndRemoveIt(appDbContext);
        await appDbContext.Database.EnsureCreatedAsync();
    }
}