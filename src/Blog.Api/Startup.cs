using Blog.Api.Auth;
using Blog.Api.Filters;
using Blog.Api.Middleware;
using Blog.Infrastructure;
using Blog.Infrastructure.Validator.FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

public class Startup
{
   
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("PostgreSqlConnection");
     
        services.AddDbContextWithPostgresql(connectionString);
        services.AddServices();
        services.AddRepositories();

        services.AddAuthentication("XUser")
           .AddScheme<XUserAuthenticationOptions, XUserAuthenticationHandler>("XUser", null);
        
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            })
            .AddFluentValidation(fv =>
            {
                fv.DisableDataAnnotationsValidation = true;
                fv.RegisterValidatorsFromAssemblyContaining<CreateAuthorRequestValidator>();
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}