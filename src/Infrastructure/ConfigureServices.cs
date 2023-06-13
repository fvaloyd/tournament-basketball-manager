using Application;
using Domain.Common;
using Infrastructure.Common;
using Infrastructure.Sql.Context;
using Infrastructure.Sql.Repositories;
using Microsoft.Extensions.Configuration;
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
    }
}