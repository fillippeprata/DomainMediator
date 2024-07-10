using DomainMediator.ExampleContext.Domain.Entities.Users;
using DomainMediator.ExampleContext.Domain.Entities.Users.ObjectValues;

namespace DomainMediator.EntityFramework.Example.Users;

public class UserEntity : IUserProperties
{
    public string JoinedRoles { get; set; } = string.Empty;
    public bool IsEmailNotificationAllowed { get; set; }
    public bool IsSmsNotificationAllowed { get; set; }
    public bool IsPushNotificationAllowed { get; set; }
    public Guid Id { get; init; }
    public string CallAs { get; set; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string[] Roles => JoinedRoles.Split(',');

    public NotificationSettings NotificationSettings => new()
    {
        IsEmailNotificationAllowed = IsEmailNotificationAllowed,
        IsSmsNotificationAllowed = IsSmsNotificationAllowed,
        IsPushNotificationAllowed = IsPushNotificationAllowed
    };
}
