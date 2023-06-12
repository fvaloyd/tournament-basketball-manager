using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ApplicationReference>());

        services.AddSingleton<ITeamMatchMaker, RandomTeamMatchMaker>();
    }
}