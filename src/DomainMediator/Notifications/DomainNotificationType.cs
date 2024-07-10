namespace DomainMediator.Notifications;

public enum DomainNotificationType
{
    Information = 0,
    Warning = 1,
    BadRequest = 2,
    SystemError = 3,
    NotFound = 4,
    Forbidden = 5,
    SuccessfullyCreated = 6,
    SuccessfullyUpdated = 7,
    SuccessfullyDeleted = 8
}