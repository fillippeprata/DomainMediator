using System.Diagnostics.CodeAnalysis;
using DomainMediator.ExampleContext.Domain.Entities.Users.Commands;
using DomainMediator.ExampleContext.Domain.Entities.Users.Queries;

namespace DomainMediator.UnitTests.Example.DomainMediator;

[ExcludeFromCodeCoverage]
public class GeneralMediatorTests
{
    [Fact]
    public async Task WhenTheMediatorIsBlocked_ThenNotExecuteACommand()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;
        mediator.AddNotification(new Exception("Blocked"));

        // Act
        await mediator.Exec(new UpdateUserCommand());

        // Assert
        Assert.Single(mediator.Notifications);
        Assert.True(mediator.ContainsNotification("Blocked"));
    }

    [Fact]
    public async Task WhenTheMediatorIsBlocked_ThenNotExecuteACommandWithResponse()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;
        mediator.AddNotification(new Exception("Blocked"));

        // Act
        await mediator.Exec(new AddUserCommand());

        // Assert
        Assert.Single(mediator.Notifications);
        Assert.True(mediator.ContainsNotification("Blocked"));
    }

    [Fact]
    public async Task WhenSendQuery_ThenCheckIfBlocked()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;
        mediator.AddNotification(new Exception("Blocked"));

        // Act
        var response = await mediator.Get(new FindUserQuery());

        // Assert
        Assert.Null(response);
    }
}