using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DomainMediator.EntityFramework;
using DomainMediator.EntityFramework.Example;
using DomainMediator.ExampleContext;
using DomainMediator.Telemetry;
using DomainMediator.Tests;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DomainMediator.UnitTests.Example;

[ExcludeFromCodeCoverage]
internal class TestProvider
{
    private TestingFactory TestingFactory { get; } = TestingFactory.Create(AddDependencies);
    public Mediator Mediator => TestingFactory.GetService<Mediator>();

    public T GetService<T>() where T : class
    {
        return TestingFactory.GetService<T>();
    }

    private static void AddDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        //Serilog Startup
        Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.TestCorrelator().CreateLogger();
        serviceCollection.AddSingleton<IDomainLogger>(new DomainSerilog());

        serviceCollection.AddContextExampleDependencies();
        serviceCollection.AddContextExampleEntityFrameworkDependencies(
            new DbContextOptionsBuilder<DomainDbContext>()
                .UseInMemoryDatabase("ContextExampleTests").Options);
        serviceCollection.RegisterAssemblyForAllPackages(Assembly.GetExecutingAssembly());
    }
}