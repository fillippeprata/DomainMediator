using AutoMapper;
using DomainMediator.DataBase;
using DomainMediator.ExampleContext.Domain.Entities.Users.Commands;
using DomainMediator.ExampleContext.Domain.Entities.Users.Events;
using DomainMediator.ExampleContext.Domain.Entities.Users.ObjectValues;
using DomainMediator.ExampleContext.Domain.Entities.Users.Repository;
using DomainMediator.Notifications;

namespace DomainMediator.ExampleContext.Domain.Entities.Users;

public interface IUserProperties : IDbEntity
{
    string CallAs { get; }
    string UserName { get; }
    string Password { get; }
    string[] Roles { get; }
    NotificationSettings NotificationSettings { get; }
}

public interface IUser : IUserProperties
{
    Task UpdateAndSaveAsync(UpdateUserCommand userCommand);
    Task UpdateNotificationSettingsAndSave(UpdateUserNotificationSettingsCommand command);
    Task DeleteAndSaveAsync(DeleteUserCommand command);
}

public class UserFactory(Mediator _mediator, IMapper _mapper, IUserStorage _storage)
{
    public async Task<UserResponse?> AddUserAndSaveAsync(AddUserCommand command)
    {
        var user = _mapper.Map<UserImp>(command);
        user.Id = Guid.NewGuid();

        var response = await _storage.AddAndSaveAsync(user);

        _mediator.Publish(new UserAddedEvent { User = user });

        _mediator.AddNotification("User created successfully.", DomainNotificationType.SuccessfullyCreated);

        return _mapper.Map<UserResponse>(response);
    }

    public async Task<IUser?> FindUserAsync(Guid userId)
    {
        var userProperties = await _storage.GetByIdAsync(userId);
        if (userProperties == null) return null;

        var user = _mapper.Map<UserImp>(userProperties);
        user.DependencyInjection(_mapper, _storage);
        return user;
    }

    public class UserImp : IUser
    {
        private IMapper? _mapper;

        private IUserStorage? _storage;

        public async Task UpdateAndSaveAsync(UpdateUserCommand userCommand)
        {
            var editedUser = this;
            await _storage!.UpdateAndSaveAsync(_mapper!.Map(userCommand, editedUser));
        }

        public async Task UpdateNotificationSettingsAndSave(UpdateUserNotificationSettingsCommand command)
        {
            var notificationSettings = _mapper!.Map<NotificationSettings>(command);
            await _storage!.UpdateNotificationSettingsAndSave(command.UserId, notificationSettings);
        }

        public async Task DeleteAndSaveAsync(DeleteUserCommand command)
        {
            await _storage!.DeleteAndSaveAsync(command.UserId);
        }

        public void DependencyInjection(IMapper mapper, IUserStorage storage)
        {
            _storage = storage;
            _mapper = mapper;
        }

        #region Properties

        public Guid Id { get; set; }
        public string CallAs { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string[] Roles { get; set; } = [];
        public NotificationSettings NotificationSettings { get; set; } = new();

        #endregion
    }
}
