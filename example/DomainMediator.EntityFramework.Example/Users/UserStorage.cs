using AutoMapper;
using DomainMediator.ExampleContext.Domain.Entities.Users;
using DomainMediator.ExampleContext.Domain.Entities.Users.ObjectValues;
using DomainMediator.ExampleContext.Domain.Entities.Users.Repository;
using DomainMediator.Notifications;
using Microsoft.EntityFrameworkCore;

namespace DomainMediator.EntityFramework.Example.Users;

internal class UserStorage(IMapper _mapper, Mediator _mediator, ExampleContextDbContext _context)
    : DbSetStorage<UserEntity>(_context), IUserStorage
{
    public async Task<IUserProperties> AddAndSaveAsync(IUserProperties user)
    {
        var entity = _mapper.Map<UserEntity>(user);
        await AddEntityAndSaveAsync(entity);
        return entity;
    }

    public async Task<IUserProperties?> GetByIdAsync(Guid id)
    {
        return await GetUserEntityByIdAsync(id);
    }

    private async Task<UserEntity?> GetUserEntityByIdAsync(Guid entityId)
    {
        var user = await FindEntityByIdAsync(entityId);
        if (user == null)
            _mediator.AddNotification("User not found.", DomainNotificationType.NotFound);
        return user;
    }

    public async Task UpdateAndSaveAsync(IUserProperties entity)
    {
        var originalUser = await GetUserEntityByIdAsync(entity.Id);
        if (originalUser != null)
        {
            originalUser.CallAs = entity.CallAs;
            originalUser.JoinedRoles = string.Join(",", entity.Roles);
            originalUser.Password = entity.Password;
            UpdateNotificationSettingsEntity(originalUser, entity.NotificationSettings);

            await UpdateEntityAndSaveAsync(originalUser);

            _mediator.AddNotification("User updated successfully.", DomainNotificationType.SuccessfullyUpdated);
        }
    }

    public async Task UpdateNotificationSettingsAndSave(Guid userId, NotificationSettings notificationSettings)
    {
        var originalUser = (await GetUserEntityByIdAsync(userId))!;

        UpdateNotificationSettingsEntity(originalUser, notificationSettings);

        await UpdateEntityAndSaveAsync(originalUser);

        _mediator.AddNotification("User Notification Settings updated successfully.", DomainNotificationType.SuccessfullyUpdated);
    }

    public async Task<IUserProperties?> GetByUserName(string userName)
    {
        var user = await EntitySet.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user == null)
            _mediator.AddNotification("User name not found.", DomainNotificationType.NotFound);
        return user;
    }

    public async Task DeleteAndSaveAsync(Guid userId)
    {
        await DeleteByIdAndSaveAsync(userId);
        _mediator.AddNotification("User deleted successfully.", DomainNotificationType.SuccessfullyDeleted);
    }

    private static void UpdateNotificationSettingsEntity(UserEntity entity, NotificationSettings notificationSettings)
    {
        entity.IsPushNotificationAllowed = notificationSettings.IsPushNotificationAllowed;
        entity.IsEmailNotificationAllowed = notificationSettings.IsEmailNotificationAllowed;
        entity.IsSmsNotificationAllowed = notificationSettings.IsSmsNotificationAllowed;
    }
}
