using System.Reflection;
using DomainMediator.EntityFramework.Example.Users;
using DomainMediator.ExampleContext.Domain.Entities.Users.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DomainMediator.EntityFramework.Example;

public static class DependencyInjection
{
    public static void AddContextExampleEntityFrameworkDependencies(this IServiceCollection services, DbContextOptions<DomainDbContext> options)
    {
        services.AddSingleton(options);
        services.AddScoped<ExampleContextDbContext>();

        services.AddScoped<IUserStorage, UserStorage>();
        services.AddScoped<IUserQuery, UserQuery>();

        services.RegisterAssemblyForAllPackages(Assembly.GetExecutingAssembly());
    }
}
