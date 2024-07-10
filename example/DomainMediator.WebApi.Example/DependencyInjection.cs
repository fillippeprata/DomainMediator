using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DomainMediator.EntityFramework;
using DomainMediator.EntityFramework.Example;
using DomainMediator.ExampleContext;
using Microsoft.EntityFrameworkCore;

namespace DomainMediator.WebApi.Example;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddExampleWebApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddContextExampleDependencies();
        services.AddContextExampleEntityFrameworkDependencies(new DbContextOptionsBuilder<DomainDbContext>()
            .UseInMemoryDatabase("ContextExampleWebApi").Options);
        services.RegisterAssemblyForAllPackages(Assembly.GetExecutingAssembly());
    }
}