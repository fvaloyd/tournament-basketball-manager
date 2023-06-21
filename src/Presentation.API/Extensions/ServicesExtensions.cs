using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;
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
}
