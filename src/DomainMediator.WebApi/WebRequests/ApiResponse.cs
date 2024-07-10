using DomainMediator.Notifications;

namespace DomainMediator.WebApi.WebRequests;

public class ApiResponse<T>(ScopedNotifications notifications)
{
    public ApiResponse(T? response, Mediator mediator) : this(mediator)
    {
        Response = response;
    }

    public ApiResponse(Mediator mediator) : this(mediator.ScopedNotifications)
    {
    }

    public bool Success { get; private set; } = notifications.Unblocked;
    public bool ContainsUserNotification { get; private set; } = notifications.ContainsUserNotification;
    public IReadOnlyCollection<DomainNotification> Notifications { get; private set; } = notifications.List;
    public T? Response { get; private set; }
}

public class ApiResponse : ApiResponse<object?>
{
    public ApiResponse(Mediator mediator) : base(mediator)
    {
    }

    public ApiResponse(ScopedNotifications notifications) : base(notifications)
    {
    }
}