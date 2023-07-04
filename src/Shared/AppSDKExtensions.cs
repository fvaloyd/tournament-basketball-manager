using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Shared;
public static class AppSDKExtensions
{
    public static void AddApiRefitClients(this IServiceCollection services, string baseUrl)
    {
        services.AddRefitClient<IOrganizerClient>().ConfigureHttpClient(cfg => cfg.BaseAddress = new Uri(baseUrl));
        services.AddRefitClient<IPlayerClient>().ConfigureHttpClient(cfg => cfg.BaseAddress = new Uri(baseUrl));
        services.AddRefitClient<IManagerClient>().ConfigureHttpClient(cfg => cfg.BaseAddress = new Uri(baseUrl));
    }
}