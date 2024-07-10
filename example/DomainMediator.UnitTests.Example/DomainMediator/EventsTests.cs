using System.Diagnostics.CodeAnalysis;
using DomainMediator.EntityFramework.Example.Users;
using DomainMediator.ExampleContext.Domain.Entities.Users.Events;

namespace DomainMediator.UnitTests.Example.DomainMediator;

[ExcludeFromCodeCoverage]
public class EventsTests
{
    [Fact]
    public async Task AddAnEventAndWaitForIt()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        // Act
        await mediator.PublishAndWait(new UserAddedEvent { User = new UserEntity() });

        // Assert
        Assert.Single(mediator.Notifications);
    }

    [Fact]
    public async Task AddAnEventAndWaitWithError()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;
        mediator.AddNotification(new Exception("Mock Error"));

        // Act
        await mediator.PublishAndWait(new UserAddedEvent { User = new UserEntity() });

        // Assert
        Assert.Single(mediator.Notifications);
        Assert.True(mediator.ContainsSystemError);
    }

    [Fact]
    public void AddAnEventAndNotWaitForIt()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        // Act
        mediator.Publish(new UserAddedEvent { User = new UserEntity() });

        // Assert
        Thread.Sleep(1000);
        Assert.Single(mediator.Notifications);
    }

    [Fact]
    public void AddAnEventAndNotWaitWithError()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;
        mediator.AddNotification(new Exception("Mock Error"));

        // Act
        mediator.Publish(new UserAddedEvent { User = new UserEntity() });

        // Assert
        Assert.Single(mediator.Notifications);
        Assert.True(mediator.ContainsSystemError);
    }
}