using Serilog;

namespace DomainMediator.Telemetry;

public class DomainSerilog : IDomainLogger
{
    public void DomainLogModel(DomainLogModel logModel)
    {
        InsertLog(logModel);
    }

    public void Information(string message, Guid? correlationId = null, Guid? userId = null)
    {
        InsertLog(new DomainLogModel
        {
            DomainLogType = DomainLogType.Information,
            Message = message,
            CorrelationId = correlationId,
            UserId = userId
        });
    }

    public void Warning(string message, Guid? correlationId = null, Guid? userId = null)
    {
        {
            InsertLog(new DomainLogModel
            {
                DomainLogType = DomainLogType.Warning,
                Message = message,
                CorrelationId = correlationId,
                UserId = userId
            });
        }
    }

    public void Error(string message, Guid? correlationId = null, Guid? userId = null)
    {
        {
            InsertLog(new DomainLogModel
            {
                Message = message,
                DomainLogType = DomainLogType.Error,
                CorrelationId = correlationId,
                UserId = userId
            });
        }
    }

    public void Error(Exception ex, Guid? correlationId = null, Guid? userId = null)
    {
        {
            InsertLog(new DomainLogModel
            {
                Message = ex.Message,
                DomainLogType = DomainLogType.Error,
                Exception = ex,
                CorrelationId = correlationId,
                UserId = userId
            });
        }
    }

    private static void InsertLog(DomainLogModel logModel)
    {
        var correlationId = "No correlation Id.";
        if (logModel.CorrelationId.HasValue) correlationId = $"Correlation Id: {logModel.CorrelationId}.";

        var userIdValue = "No user Id.";
        if (logModel.UserId.HasValue) userIdValue = $"User Id: {logModel.UserId}.";

        var message = $"{correlationId} {userIdValue} {logModel.Message}";

        switch (logModel.DomainLogType)
        {
            case DomainLogType.Information:
                Log.Information(message);
                break;
            case DomainLogType.Warning:
                Log.Warning(message);
                break;
            case DomainLogType.Error:
            {
                if (logModel.Exception != null)
                    Log.Error(logModel.Exception, message);
                else
                    Log.Error(message);
                break;
            }
        }
    }
}