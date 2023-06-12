using Application;
using Domain.Common;
using Infrastructure.Common;
using Infrastructure.Sql.Context;
using Infrastructure.Sql.Repositories;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationLayerAssemby = typeof(ApplicationReference).Assembly;
        var infrastructureAssembly = typeof(InfrastructureReference).Assembly;
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(applicationLayerAssemby, infrastructureAssembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddSqlServer<TournamentBasketballManagerDbContext>(configuration.GetConnectionString("TournamentBasketballManagerDb"));
        services.AddSingleton<IUnitOfWorkFactory, HandlersUnitOfWorkFactory>();

        services.AddScoped<IUnitOfWork, SqlUnitOfWork>();
        services.AddScoped<IUnitOfWork, MongoUnitOfWork>();

        services.AddSingleton<ILoggerManager, SerilogLoggerManager>();

        services.ConfigureOptions<MongoDatabaseSettingsSetup>();
    }
}