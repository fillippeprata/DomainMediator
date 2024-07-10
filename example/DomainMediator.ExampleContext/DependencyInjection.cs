using System.Reflection;
using DomainMediator.ExampleContext.Domain.Entities.Users;
using DomainMediator.ExampleContext.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DomainMediator.ExampleContext;

public static class DependencyInjection
{
    public static void AddContextExampleDependencies(this IServiceCollection services)
    {
        services.AddScoped<UserFactory>();
        services.AddScoped<IAuthenticationService, AuthenticationServiceImp>();

        services.RegisterAssemblyForAllPackages(Assembly.GetExecutingAssembly());
    }
}