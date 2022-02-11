using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Blog.FunctionalTests.LoggerUtil;
using Blog.Infrastructure;
using Blog.Infrastructure.Db.EF;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using TestSupport.Helpers;
using Xunit.Abstractions;

namespace Blog.FunctionalTests;

internal class TestClient : WebApplicationFactory<Program>
{
    private readonly ITestOutputHelper _testOutputHelper;
    public readonly string Environment;

    private TestClient(ITestOutputHelper testOutputHelper, string environment = "FunctionalTests")
    {
        _testOutputHelper = testOutputHelper;
        Environment = environment;
    }

    public static HttpClient CreateHttpClient(ITestOutputHelper testOutputHelper, string? connectionString = null)
    {
        if (string.IsNullOrWhiteSpace(connectionString)) return new TestClient(testOutputHelper).CreateClient();
        var server = new TestClient(testOutputHelper).WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, configurationBuilder) =>
            {
                configurationBuilder.AddInMemoryCollection(new[]
                    {
                        new KeyValuePair<string, string>("ConnectionStrings:PostgreSqlConnection", connectionString)
                    }
                );
            });
            builder.ConfigureServices(s => { s.AddDbContextWithPostgresql(connectionString); });
        }).Server;
        return server.CreateClient();
    }


    public static BlogDbContext GetDbContext(string databaseName, out string connectionString)
    {
        var config = AppSettings.GetConfiguration();
        connectionString = new NpgsqlConnectionStringBuilder(
            config.GetConnectionString("PostgreSqlConnection"))
        {
            Database = $"{databaseName}"
        }.ToString();
        var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        //x => x.MigrationsAssembly(typeof(StartupSetupDbContext).Namespace));
        return new BlogDbContext(optionsBuilder.Options);
    }

    public static async Task PrepareDatabase(BlogDbContext appDbContext)
    {
        await CheckDatabaseAndRemoveIt(appDbContext);
        await appDbContext.Database.EnsureCreatedAsync();
    }

    public static async Task PrepareDatabaseWithMigrations(BlogDbContext appDbContext)
    {
        await CheckDatabaseAndRemoveIt(appDbContext);
        await appDbContext.Database.MigrateAsync();
    }

    public static IServiceProvider GetServiceProvider(ITestOutputHelper testOutputHelper)
    {
        return new TestClient(testOutputHelper).Server.Services;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(Environment);

        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.Services.AddSingleton<ILoggerProvider>(_ => new XUnitLoggerProvider(_testOutputHelper));
            loggingBuilder.AddConsole();
        });
        return base.CreateHost(builder);
    }

    public static async Task CheckDatabaseAndRemoveIt(BlogDbContext appDbContext)
    {
        if (await appDbContext.Database.CanConnectAsync().ConfigureAwait(false))
            await appDbContext.Database.EnsureDeletedAsync().ConfigureAwait(false);
    }
}