using AutoMapper;
using DomainMediator.ExampleContext.Domain.Entities.Users.Commands;
using DomainMediator.ExampleContext.Domain.Entities.Users.ObjectValues;

namespace DomainMediator.ExampleContext.Domain.Entities.Users;

public class UsersMapper : Profile
{
    public UsersMapper()
    {
        CreateMap<AddUserCommand, UserFactory.UserImp>();
        CreateMap<UpdateUserCommand, UserFactory.UserImp>();
        CreateMap<UpdateUserNotificationSettingsCommand, NotificationSettings>();

        CreateMap<IUserProperties, UserFactory.UserImp>();

        CreateMap<IUserProperties, UserResponse>()
            .ForMember(x => x.UserId, y => y.MapFrom(x => x.Id));
    }
}