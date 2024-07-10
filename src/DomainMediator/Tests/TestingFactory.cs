using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DomainMediator.Tests;

public class TestingFactory
{
    private TestingFactory(Action<IServiceCollection, IConfiguration> addDependencies)
    {
        var configuration = InitSettings();

        ServiceCollection.AddArchitectureDependencies();

        addDependencies(ServiceCollection, configuration);

        Provider = ServiceCollection.BuildServiceProvider();

        #region Local methods

        IConfiguration InitSettings()
        {
            var testSettings = new ConfigurationBuilder().AddJsonFile("app.test.settings.json", true).Build();
            ServiceCollection.AddSingleton<IConfiguration>(s => testSettings);
            return testSettings;
        }

        #endregion
    }

    private ServiceProvider Provider { get; }
    private ServiceCollection ServiceCollection { get; set; } = [];

    public static TestingFactory Create(Action<IServiceCollection, IConfiguration> addDependencies)
    {
        return new TestingFactory(addDependencies);
    }

    public T GetService<T>() where T : class
    {
        return Provider.GetRequiredService<T>();
    }
}
