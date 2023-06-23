using Infrastructure.Sql.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Threading.RateLimiting;

namespace Presentation.API.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection ConfigureRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(cfg =>
        {
            cfg.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>
                RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter", _ =>
                    new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 100,
                        QueueLimit = 2,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        Window = TimeSpan.FromMinutes(1)
                    }));
        });
        return services;
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", cfg =>
            {
                cfg.AllowAnyOrigin();
                cfg.AllowAnyHeader();
                cfg.AllowAnyMethod();
                cfg.WithExposedHeaders("x-pagination");
            });
        });
        return services;
    }

    public static IServiceCollection ConfigureOutputCache(this IServiceCollection services)
    {
        services.AddOutputCache(cfg =>
        {
            cfg.AddPolicy("OutputCachePolicy", builder =>
            {
                builder.Expire(TimeSpan.FromSeconds(10));
            });
        });
        return services;
    }

    public static IServiceCollection ConfigureSqlServerDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TournamentBasketballManagerDbContext>(
            opt => opt.UseSqlServer(
                connectionString: configuration.GetConnectionString("TournamentBasketballManagerDb"),
                sqlServerOptionsAction: s => s.MigrationsAssembly(typeof(Program).Assembly.FullName)),
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient);
        return services;
    }

    public static void MigrateDatabase(this WebApplication app)
    {
        using var appContext = app.Services.GetRequiredService<TournamentBasketballManagerDbContext>();
        try
        {
            if (appContext.Database.EnsureCreated())
            {
                appContext.Database.Migrate();
            }
        }
        catch (SqlException)
        {
            app.Logger.LogError("The server was not found or was not accessible.");
        }
    }

    public static void ConfigureSerilog(this IServiceCollection services)
    {
        var currentDir = Directory.GetCurrentDirectory();
        var logsDirInfo = Directory.CreateDirectory(Path.Combine(currentDir, "logs"));
        var logsFilePath = Path.Combine(logsDirInfo.ToString(), "logs.txt");
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(logsFilePath)
            .CreateLogger();

        services.AddSerilog(Log.Logger);
    }
}