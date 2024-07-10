namespace DomainMediator.Telemetry;

public interface IDomainLogger
{
    void DomainLogModel(DomainLogModel logModel);
    void Information(string message, Guid? correlationId = null, Guid? userId = null);
    void Warning(string message, Guid? correlationId = null, Guid? userId = null);
    void Error(string message, Guid? correlationId = null, Guid? userId = null);
    void Error(Exception ex, Guid? correlationId = null, Guid? userId = null);
}