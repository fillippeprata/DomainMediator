using System.Diagnostics.CodeAnalysis;

namespace DomainMediator.ExampleContext.Domain.Entities.Users.ObjectValues;

[ExcludeFromCodeCoverage]
public record NotificationSettings
{
    public bool IsEmailNotificationAllowed { get; init; }
    public bool IsSmsNotificationAllowed { get; init; }
    public bool IsPushNotificationAllowed { get; init; }
}