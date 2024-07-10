using System.Diagnostics.CodeAnalysis;
using System.Net;
using DomainMediator.ExampleContext.Domain.Entities.Users;
using DomainMediator.ExampleContext.Domain.Entities.Users.Commands;
using DomainMediator.Notifications;
using DomainMediator.Tests;
using DomainMediator.WebApi.Example.Features.Auth;
using DomainMediator.WebApi.Jwt;

namespace DomainMediator.UnitTests.Example.DomainMediator;

[ExcludeFromCodeCoverage]
public class DomainWebApiTests(AppTestingProvider app) : IClassFixture<AppTestingProvider>
{
    private const string _baseUrl = "/v1/example";
    private const string _users = $"{_baseUrl}/users";
    private readonly HttpClient _client = app.CreateClient().Init();

    [Fact]
    public async Task TestingErrorMiddleware()
    {
        #region Add an user

        var addUserCommand = new AddUserCommand
        {
            CallAs = "Exception Test User",
            Password = "password",
            UserName = "exception.test"
        };
        await _client.Send(_users, new WebAppTestModel { Content = addUserCommand, HttpMethod = TestHttpMethod.Post });

        #endregion

        #region Authenticate the user

        var authenticateUserCommand = new AuthenticateUserCommand
        {
            Password = addUserCommand.Password,
            UserName = addUserCommand.UserName
        };

        var authResponse = await _client.Send($"{_baseUrl}/auth", new WebAppTestModel { Content = authenticateUserCommand, HttpMethod = TestHttpMethod.Post });

        #endregion

        _client.SetAccessToken(authResponse!.ResponseData<JwtResponse>()!.AccessToken!);

        //Arrange
        var testModel = new WebAppTestModel { EnsureSuccess = false };

        //Act
        var response = await _client.Send($"{_baseUrl}/error", testModel);
        var notifications = response!.GetNotifications();

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.HttpResponse?.StatusCode);
        Assert.False(response.Success);
        Assert.Single(notifications!);
        Assert.Equal("Test Error Middleware.", notifications![0].Message);
        Assert.Equal(DomainNotificationType.SystemError, notifications[0].NotificationTypeEnum);
    }

    [Fact]
    public async Task TestingUnauthorizedRoute()
    {
        //Arrange
        var testModel = new WebAppTestModel { EnsureSuccess = false };

        //Act
        var response = await _client.Send($"{_baseUrl}/users/{Guid.NewGuid()}", testModel);
        var notifications = response!.GetNotifications();
        var responseData = response.ResponseData<object>();

        // Assert
        Assert.Null(notifications);
        Assert.Null(responseData);
        Assert.Equal(HttpStatusCode.Unauthorized, response.HttpResponse?.StatusCode);
        Assert.False(response.Success);
    }
}
