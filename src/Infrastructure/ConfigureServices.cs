using Domain.Common;
using Domain.Players;
using Domain.Managers;
using Domain.Organizers;
using Infrastructure.Common;
using Infrastructure.Sql.Context;
using Infrastructure.Sql.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer<TournamentBasketballManagerDbContext>(configuration.GetConnectionString("TournamentBasketballManagerDb"));
        services.AddSingleton<IUnitOfWorkFactory, HandlersUnitOfWorkFactory>();

        services.AddScoped<IPlayerRepository, SqlPlayerRepository>();
        services.AddScoped<IManagerRepository, SqlManagerRepository>();
        services.AddScoped<IOrganizerRepository, SqlOrganizerRepository>();
        services.AddScoped<ITeamRepository, SqlTeamRepository>();
        services.AddScoped<IUnitOfWork, SqlUnitOfWork>();
    }
}