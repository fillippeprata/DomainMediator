using System.Diagnostics.CodeAnalysis;
using DomainMediator.Notifications;

namespace DomainMediator.UnitTests.Example.DomainMediator;

[ExcludeFromCodeCoverage]
public class NotificationsTests
{
    [Fact]
    public void WhenExistsANotification_ShouldBeAbleToReadNotificationAtMediator()
    {
        // Arrange
        var message = "Test Notification";
        var mediator = new TestProvider().Mediator;

        // Act
        mediator.AddNotification(message, DomainNotificationType.BadRequest);

        // Assert
        Assert.Single(mediator.Notifications);
        Assert.True(mediator.ContainsNotification(message));
    }

    [Fact]
    public void CheckExceptionNotification()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        var exception = new Exception("Exception Test");
        // Act
        mediator.AddNotification(exception);

        // Assert
        Assert.True(mediator.ContainsNotification(exception.Message));
        Assert.True(mediator.ContainsSystemError);
        Assert.True(mediator.Blocked);
    }

    [Fact]
    public void CheckContainsUserNotification()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        // Act
        mediator.AddNotification("Test Notification", DomainNotificationType.BadRequest);

        // Assert
        Assert.True(mediator.ContainsUserNotification);
    }

    [Fact]
    public void CheckNotContainsUserNotification()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        var notification = new DomainNotification
        {
            Message = "Test Notification", NotificationTypeEnum = DomainNotificationType.BadRequest, ShowToUser = false,
            Property = "Field1"
        };
        // Act
        mediator.AddNotification(notification);

        // Assert
        Assert.False(mediator.ContainsUserNotification);
        Assert.Equal(DomainNotificationType.BadRequest.ToString(), notification.NotificationTypeName);
        Assert.NotNull(notification.Property);
    }

    [Fact]
    public void CheckSystemErrors()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        // Act
        mediator.AddNotification("Test Notification", DomainNotificationType.SystemError);

        // Assert
        Assert.True(mediator.ContainsSystemError);
    }

    [Fact]
    public void CheckNoSystemErrors()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        // Act
        mediator.AddNotification("Test Notification", DomainNotificationType.Information);

        // Assert
        Assert.True(mediator.NoSystemErrors);
    }

    [Fact]
    public void CheckIfSystemErrorsAreBlockages()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        // Act
        mediator.AddNotification("Test Notification", DomainNotificationType.SystemError);

        // Assert
        Assert.True(mediator.Blocked);
    }

    [Fact]
    public void CheckIfBadRequestsAreBlockages()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        // Act
        mediator.AddNotification("Test Notification", DomainNotificationType.BadRequest);

        // Assert
        Assert.True(mediator.Blocked);
    }

    [Fact]
    public void CheckIfNotFoundNotificationAreBlockages()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        // Act
        mediator.AddNotification("Test Notification", DomainNotificationType.NotFound);

        // Assert
        Assert.True(mediator.Blocked);
    }

    [Fact]
    public void CheckIfUnauthorizedNotificationAreBlockages()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        // Act
        mediator.AddNotification("Test Notification", DomainNotificationType.Forbidden);

        // Assert
        Assert.True(mediator.Blocked);
    }

    [Fact]
    public void CheckInformationIsNotBlocking()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        // Act
        mediator.AddNotification("Test Notification", DomainNotificationType.Information);

        // Assert
        Assert.True(mediator.Unblocked);
    }

    [Fact]
    public void CheckIfWarningNotificationAreBlockages()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        // Act
        mediator.AddNotification("Test Notification", DomainNotificationType.Warning);

        // Assert
        Assert.True(mediator.Unblocked);
    }
}