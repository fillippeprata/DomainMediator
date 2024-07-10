using System.Diagnostics.CodeAnalysis;
using DomainMediator.Exceptions;

namespace DomainMediator.Notifications;

public abstract class ScopedNotifications
{
    protected List<DomainNotification> Notifications { get; } = [];

    public abstract void Add(Exception ex);
    public abstract void Add(DomainNotification notification);
    public abstract void Add(string message, DomainNotificationType notificationType);

    #region Properties

    public List<DomainNotification> List => Notifications;

    public bool ContainsSystemError =>
        Notifications.Exists(x => x.NotificationTypeEnum == DomainNotificationType.SystemError);

    public bool NoSystemErrors => !ContainsSystemError;

    public bool ContainsUserNotification => Notifications.Exists(x => x.ShowToUser);

    public bool ContainsBadRequestNotification =>
        Notifications.Exists(x => x.NotificationTypeEnum == DomainNotificationType.BadRequest);

    public bool ContainsNotFoundNotification =>
        Notifications.Exists(x => x.NotificationTypeEnum == DomainNotificationType.NotFound);

    public bool ContainsForbiddenNotification =>
        Notifications.Exists(x => x.NotificationTypeEnum == DomainNotificationType.Forbidden);

    public bool ContainsSuccessfullyCreatedNotification =>
        Notifications.Exists(x => x.NotificationTypeEnum == DomainNotificationType.SuccessfullyCreated);

    public bool Unblocked => NoSystemErrors && !ContainsBadRequestNotification && !ContainsNotFoundNotification &&
                             !ContainsForbiddenNotification;

    public bool Blocked => !Unblocked;

    #endregion
}

internal class ScopedNotificationsImp : ScopedNotifications
{
    public override void Add(Exception ex)
    {
        Notifications.Add(new DomainNotification
        {
            Message = ex.RootExceptionText(), NotificationTypeEnum = DomainNotificationType.SystemError,
            ShowToUser = false
        });
        TrackErrorIfDevelopmentEnvironment(ex);
    }

    [ExcludeFromCodeCoverage]
    private void TrackErrorIfDevelopmentEnvironment(Exception ex)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (environment == "Development" && ex.StackTrace != null)
            Notifications.Add(new DomainNotification
            {
                Message = ex.StackTrace, NotificationTypeEnum = DomainNotificationType.Information, ShowToUser = false,
                Property = "Exception.StackTrace"
            });
    }

    public override void Add(DomainNotification notification)
    {
        Notifications.Add(notification);
    }

    public override void Add(string message, DomainNotificationType notificationType)
    {
        Notifications.Add(new DomainNotification { Message = message, NotificationTypeEnum = notificationType });
    }
}