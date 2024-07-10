using System.Reflection;
using DomainMediator.Notifications;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DomainMediator;

public static class DependencyInjection
{
    public static void AddArchitectureDependencies(this IServiceCollection services)
    {
        services.AddScoped<Mediator, MediatorImp>();
        services.AddScoped<ScopedNotifications, ScopedNotificationsImp>();

        services.RegisterAssemblyForMediator(Assembly.GetExecutingAssembly());
    }

    private static void RegisterAssemblyForMediator(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
    }

    public static void RegisterAssemblyForAllPackages(this IServiceCollection services, Assembly assembly)
    {
        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.RegisterAssemblyForMediator(assembly);
    }
}