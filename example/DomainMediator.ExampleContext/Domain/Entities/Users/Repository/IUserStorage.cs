using DomainMediator.ExampleContext.Domain.Entities.Users.ObjectValues;

namespace DomainMediator.ExampleContext.Domain.Entities.Users.Repository;

public interface IUserStorage
{
    Task<IUserProperties> AddAndSaveAsync(IUserProperties user);
    Task<IUserProperties?> GetByIdAsync(Guid entityId);
    Task UpdateAndSaveAsync(IUserProperties entity);
    Task<IUserProperties?> GetByUserName(string userName);
    Task UpdateNotificationSettingsAndSave(Guid userId, NotificationSettings notificationSettings);
    Task DeleteAndSaveAsync(Guid userId);
}