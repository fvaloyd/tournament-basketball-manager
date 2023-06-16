using MassTransit;
using Application;
using Domain.Common;
using Infrastructure.Common;
using Infrastructure.Messaging;
using Infrastructure.Sql.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        var applicationLayerAssemby = typeof(ApplicationReference).Assembly;
        var infrastructureAssembly = typeof(InfrastructureReference).Assembly;
        services.AddAutoMapper(applicationLayerAssemby, infrastructureAssembly);

        services.AddSingleton<IUnitOfWorkFactory, HandlersUnitOfWorkFactory>();

        services.AddTransient<IUnitOfWork, SqlUnitOfWork>();
        services.AddTransient<IUnitOfWork, MongoUnitOfWork>();

        services.AddSingleton<ILoggerManager, SerilogLoggerManager>();

        services.ConfigureOptions<MongoDatabaseSettingsSetup>();
        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(InfrastructureReference).Assembly);
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.ConfigureEndpoints(ctx);
                cfg.Host("localhost", r =>
                {
                    r.Username("user");
                    r.Password("francis.123");
                });
            });
        });

        services.AddHostedService<MassTransitBackgroundService>();
    }
}