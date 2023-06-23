using Application.Common.Behaviors;
using Domain.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<ApplicationReference>();
            cfg.AddOpenBehavior(typeof(LogginBehavior<,>));
            cfg.AddOpenBehavior(typeof(UnhandledExceptionBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(typeof(ApplicationReference).Assembly);
        services.AddSingleton<ITeamMatchMaker, RandomTeamMatchMaker>();
        return services;
    }
}