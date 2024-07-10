using System.Diagnostics.CodeAnalysis;

namespace DomainMediator.Telemetry;

[ExcludeFromCodeCoverage]
public record DomainLogModel
{
    public required string Message { get; init; }
    public required DomainLogType DomainLogType { get; init; }
    public Guid? CorrelationId { get; init; }
    public Guid? UserId { get; init; }
    public Exception? Exception { get; init; }
}
