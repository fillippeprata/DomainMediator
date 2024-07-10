using System.Diagnostics.CodeAnalysis;
using DomainMediator.ExampleContext.Domain.Entities.Users.Commands;
using DomainMediator.ExampleContext.Domain.Entities.Users.Repository;
using DomainMediator.ExampleContext.Domain.Services;

namespace DomainMediator.UnitTests.Example.Users;

[ExcludeFromCodeCoverage]
public class UsersCommands
{
    [Fact]
    public async Task WhenTryingToUpdateAnNonExistentUser_ThenReturnNotFound()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;
        var updateCommand = new UpdateUserCommand();
        updateCommand.SetUserId(Guid.NewGuid());

        // Act
        await mediator.Exec(updateCommand);

        // Assert
        Assert.Single(mediator.Notifications);
        Assert.True(mediator.ContainsNotification("User not found."));
    }

    [Fact]
    public async Task Add_Authenticate_Update_Find_UpdateNotifications_And_DeleteUser()
    {
        // Arrange
        var provider = new TestProvider();
        var mediator = provider.Mediator;
        var userStorage = provider.GetService<IUserStorage>();
        var authenticationService = new TestProvider().GetService<IAuthenticationService>();

        #region Add a User

        var addUserResponse = await mediator.Exec(new AddUserCommand
        {
            CallAs = "User Test",
            UserName = "user.test",
            Password = "password"
        });
        Assert.NotNull(addUserResponse?.UserName);
        Assert.True(addUserResponse.UserId != Guid.Empty);
        Assert.True(mediator.ContainsNotification("User created successfully."));

        #endregion

        #region User Authentication

        var authenticatedUser = await authenticationService.AuthenticateAsync("wrong-user", "wrong-password");
        Assert.Null(authenticatedUser);
        authenticatedUser = await authenticationService.AuthenticateAsync(addUserResponse.UserName, "wrong-password");
        Assert.Null(authenticatedUser);
        authenticatedUser = await authenticationService.AuthenticateAsync(addUserResponse.UserName, "password");
        Assert.NotNull(authenticatedUser);

        #endregion

        #region Update the user

        var updateCommand = new UpdateUserCommand
        {
            CallAs = "User Test 2",
            Password = "password",
            Roles = ["Admin"]
        };
        updateCommand.SetUserId(addUserResponse.UserId);
        await mediator.Exec(updateCommand);
        Assert.True(mediator.ContainsNotification("User updated successfully."));

        #endregion

        #region Update Notification Settings

        var updateSettingsCommand = new UpdateUserNotificationSettingsCommand
        {
            IsEmailNotificationAllowed = true,
            IsPushNotificationAllowed = true,
            IsSmsNotificationAllowed = true
        };

        var updateMessage = "User Notification Settings updated successfully.";

        var updateNotificationMediator = new TestProvider().Mediator;
        updateSettingsCommand.SetUserId(Guid.NewGuid());
        await updateNotificationMediator.Exec(updateSettingsCommand);
        Assert.False(updateNotificationMediator.ContainsNotification(updateMessage));

        updateSettingsCommand.SetUserId(addUserResponse.UserId);
        await mediator.Exec(updateSettingsCommand);
        Assert.True(mediator.ContainsNotification(updateMessage));

        #endregion

        #region Find the user

        var user = await userStorage.GetByIdAsync(addUserResponse.UserId);
        Assert.NotNull(user);
        Assert.Equal("User Test 2", user.CallAs);
        Assert.True(user.NotificationSettings.IsPushNotificationAllowed);

        #endregion

        #region Delete the user

        var deleteMessage = "User deleted successfully.";
        var deleteCommand = new DeleteUserCommand();

        var deleteCommandMediator = new TestProvider().Mediator;
        deleteCommand.SetUserId(Guid.NewGuid());
        await deleteCommandMediator.Exec(deleteCommand);
        Assert.False(deleteCommandMediator.ContainsNotification(deleteMessage));

        deleteCommand.SetUserId(addUserResponse.UserId);
        await mediator.Exec(deleteCommand);
        Assert.True(mediator.ContainsNotification(deleteMessage));

        #endregion

        #region Check if user was deleted

        user = await userStorage.GetByIdAsync(addUserResponse.UserId);
        Assert.Null(user);
        Assert.True(mediator.Blocked);

        #endregion
    }
}
