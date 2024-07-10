using System.Diagnostics.CodeAnalysis;

namespace DomainMediator.Notifications;

[ExcludeFromCodeCoverage]
public record DomainNotification
{
    public required string Message { get; init; }
    public DomainNotificationType NotificationTypeEnum { get; init; }
    public string NotificationTypeName => NotificationTypeEnum.ToString();
    public bool ShowToUser { get; init; } = true;
    public string? Property { get; init; }
}