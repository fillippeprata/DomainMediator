using DomainMediator.Telemetry;
using DomainMediator.WebApi;
using DomainMediator.WebApi.Example;
using DomainMediator.WebApi.Example.Routes;
using Serilog;
using Serilog.Formatting.Compact;

//For read from configuration file: https://github.com/serilog/serilog-settings-configuration
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    //Domain Web Api Configuration
    builder.ConfigureDomainWebApiProgram(ContextExampleRoutes.DefaultVersion);

    //Serilog Startup
    builder.Services.AddSerilog();
    builder.Services.AddSingleton<IDomainLogger>(new DomainSerilog());

    //Context Example Dependencies
    builder.Services.AddExampleWebApiDependencies(builder.Configuration);

    //App Building
    var app = builder.BuildDomainWebApiProgram();
    app.AddContextExampleRoutes();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Used to allow xUnit tests to run it
public class WebApiExampleProgram;