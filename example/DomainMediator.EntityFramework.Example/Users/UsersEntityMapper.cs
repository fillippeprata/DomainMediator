using AutoMapper;
using DomainMediator.ExampleContext.Domain.Entities.Users;

namespace DomainMediator.EntityFramework.Example.Users;

internal class UsersEntityMapper : Profile
{
    public UsersEntityMapper()
    {
        CreateMap<IUserProperties, UserEntity>()
            .ForMember(x => x.JoinedRoles, y => y.MapFrom(x => string.Join(",", x.Roles)))
            .ForMember(x => x.IsPushNotificationAllowed,
                y => y.MapFrom(x => x.NotificationSettings.IsPushNotificationAllowed))
            .ForMember(x => x.IsSmsNotificationAllowed,
                y => y.MapFrom(x => x.NotificationSettings.IsSmsNotificationAllowed))
            .ForMember(x => x.IsEmailNotificationAllowed,
                y => y.MapFrom(x => x.NotificationSettings.IsEmailNotificationAllowed))
            .ReverseMap();
    }
}
