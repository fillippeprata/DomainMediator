using System.Diagnostics.CodeAnalysis;
using DomainMediator.Telemetry;
using Serilog.Sinks.TestCorrelator;

namespace DomainMediator.UnitTests.Example.DomainMediator;

[ExcludeFromCodeCoverage]
public class DomainLoggerTests
{
    [Fact]
    public void TestSerilogContext()
    {
        var domainLogger = new TestProvider().GetService<IDomainLogger>();

        using (TestCorrelator.CreateContext())
        {
            var correlationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            domainLogger.Information("My information message!", correlationId, userId);
            domainLogger.Warning("My warning message!", correlationId, userId);
            domainLogger.Error("My error message!", correlationId, userId);
            domainLogger.Error(new Exception("My exception message!"), correlationId, userId);
            domainLogger.DomainLogModel(new DomainLogModel
            {
                CorrelationId = correlationId, DomainLogType = DomainLogType.Information,
                Message = "My domain log message!", UserId = userId
            });

            var logEvents = TestCorrelator.GetLogEventsFromCurrentContext();
            Assert.NotEmpty(logEvents);
        }
    }
}