using Blog.Api.Extensions;
using Serilog;

CreateHostBuilder(args).Build().SeedData(args).Run();


static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        })
        .ConfigureAppConfiguration(a => { 
            a.AddEnvironmentVariables();
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
        

        