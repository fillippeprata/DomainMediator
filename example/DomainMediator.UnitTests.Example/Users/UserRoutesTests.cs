using System.Diagnostics.CodeAnalysis;
using DomainMediator.ExampleContext.Domain.Entities.Users;
using DomainMediator.ExampleContext.Domain.Entities.Users.Commands;
using DomainMediator.Notifications;
using DomainMediator.Tests;
using DomainMediator.WebApi.Example.Features.Auth;
using DomainMediator.WebApi.Jwt;

namespace DomainMediator.UnitTests.Example.Users;

[ExcludeFromCodeCoverage]
public class UserRoutesTests(AppTestingProvider app) : IClassFixture<AppTestingProvider>
{
    private const string _baseUrl = "/v1/example";
    private const string _users = $"{_baseUrl}/users";
    private readonly HttpClient _client = app.CreateClient().Init();

    [Fact]
    public async Task Add_Authenticate_Update_Find_UpdateNotifications_And_DeleteUser()
    {
        #region Validate Add User Command

        // Arrange
        var addUserCommand = new AddUserCommand();

        // Act
        var addUserResponse = await _client.Send(_users,
            new WebAppTestModel { Content = addUserCommand, EnsureSuccess = false, HttpMethod = TestHttpMethod.Post });
        var notifications = addUserResponse!.GetNotifications();

        // Assert
        Assert.False(addUserResponse.Success);
        Assert.NotNull(notifications);
        Assert.Equal(DomainNotificationType.BadRequest, notifications[0].NotificationTypeEnum);

        #endregion

        #region Add an user

        // Arrange
        addUserCommand = new AddUserCommand
        {
            CallAs = "WebApp Test User",
            Password = "password",
            Roles = ["Normal"],
            UserName = "web.api.test"
        };

        // Act
        addUserResponse = await _client.Send(_users,
            new WebAppTestModel { Content = addUserCommand, HttpMethod = TestHttpMethod.Post });

        // Assert
        Assert.NotNull(addUserResponse);
        var addUserData = addUserResponse?.ResponseData<UserResponse>();

        var userId = addUserData!.UserId;

        #endregion

        #region Authenticate the user

        // Arrange
        var authenticateUserCommand = new AuthenticateUserCommand
        {
            Password = addUserCommand.Password,
            UserName = addUserCommand.UserName
        };

        // Act
        var authResponse = await _client.Send($"{_baseUrl}/auth",
            new WebAppTestModel { Content = authenticateUserCommand, HttpMethod = TestHttpMethod.Post });

        // Assert
        Assert.NotNull(authResponse);
        var responseData = authResponse?.ResponseData<JwtResponse>();
        Assert.NotNull(responseData?.AccessToken);

        _client.SetAccessToken(responseData.AccessToken);

        #endregion

        #region Update the user

        // Arrange
        var updateCommand = new UpdateUserCommand
        {
            CallAs = "User Test 2",
            Password = "password2",
            Roles = ["Admin2"]
        };

        // Act
        var updateCommandResponse = await _client.Send($"{_users}/{userId}",
            new WebAppTestModel { Content = updateCommand, HttpMethod = TestHttpMethod.Put });
        notifications = updateCommandResponse!.GetNotifications();

        // Assert
        Assert.True(updateCommandResponse!.Success);
        Assert.NotNull(notifications);
        Assert.Single(notifications);
        Assert.Equal("User updated successfully.", notifications[0].Message);
        Assert.Equal(DomainNotificationType.SuccessfullyUpdated, notifications[0].NotificationTypeEnum);

        #endregion

        #region Update Notification Settings

        var updateSettingsCommand = new UpdateUserNotificationSettingsCommand
        {
            IsEmailNotificationAllowed = true,
            IsPushNotificationAllowed = true,
            IsSmsNotificationAllowed = true
        };

        var updateNotificationsSettingsCommandResponse = await _client.Send($"{_users}/{userId}/notification-settings",
            new WebAppTestModel { Content = updateSettingsCommand, HttpMethod = TestHttpMethod.Patch });
        notifications = updateNotificationsSettingsCommandResponse!.GetNotifications();

        //Assert
        Assert.True(updateNotificationsSettingsCommandResponse!.Success);
        Assert.NotNull(notifications);
        Assert.Single(notifications);
        Assert.Equal("User Notification Settings updated successfully.", notifications[0].Message);
        Assert.Equal(DomainNotificationType.SuccessfullyUpdated, notifications[0].NotificationTypeEnum);

        #endregion

        #region Find the user

        // Act
        var findUserResponse = await _client.Send($"{_users}/{userId}");
        var findUserData = findUserResponse?.ResponseData<UserResponse>();

        Assert.NotNull(findUserData);
        Assert.Equal("User Test 2", findUserData.CallAs);
        Assert.Equal("Admin2", findUserData.Roles.FirstOrDefault());

        #endregion

        #region Delete the user

        // Act
        var deleteUserResponse = await _client.Send($"{_users}/{userId}",
            new WebAppTestModel { HttpMethod = TestHttpMethod.Delete });
        notifications = deleteUserResponse!.GetNotifications();

        //Assert
        Assert.True(deleteUserResponse!.Success);
        Assert.NotNull(notifications);
        Assert.Single(notifications);
        Assert.Equal("User deleted successfully.", notifications[0].Message);
        Assert.Equal(DomainNotificationType.SuccessfullyDeleted, notifications[0].NotificationTypeEnum);

        #endregion

        #region Check if user was deleted

        // Act
        findUserResponse = await _client.Send($"{_users}/{userId}", new WebAppTestModel { EnsureSuccess = false });
        notifications = findUserResponse!.GetNotifications();

        //Assert
        Assert.False(findUserResponse!.Success);
        Assert.NotNull(notifications);
        Assert.Single(notifications);
        Assert.Equal("User not found.", notifications[0].Message);
        Assert.Equal(DomainNotificationType.NotFound, notifications[0].NotificationTypeEnum);

        #endregion
    }
}
