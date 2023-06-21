using MassTransit;
using Application;
using Domain.Common;
using Infrastructure.Common;
using Infrastructure.Messaging;
using Infrastructure.Sql.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;
public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationLayerAssemby = typeof(ApplicationReference).Assembly;
        var infrastructureAssembly = typeof(InfrastructureReference).Assembly;
        services.AddAutoMapper(applicationLayerAssemby, infrastructureAssembly);

        services.AddSingleton<IUnitOfWorkFactory, HandlersUnitOfWorkFactory>();

        services.AddTransient<IUnitOfWork, SqlUnitOfWork>();
        services.AddTransient<IUnitOfWork, MongoUnitOfWork>();

        services.AddSingleton<ILoggerManager, SerilogLoggerManager>();

        services.ConfigureOptions<MongoDatabaseSettingsSetup>();

        RabbitMQOptions rabbitOptions = new();
        configuration.GetSection("RabbitMQSettings").Bind(rabbitOptions);
        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(InfrastructureReference).Assembly);
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.ConfigureEndpoints(ctx);
                cfg.Host(rabbitOptions.Host, r =>
                {
                    r.Username(rabbitOptions.User);
                    r.Password(rabbitOptions.Password);
                });
            });
        });

        services.AddHostedService<MassTransitBackgroundService>();
    }
}