using System.Diagnostics.CodeAnalysis;
using DomainMediator.Notifications;

namespace DomainMediator.UnitTests.Example.DomainMediator;

[ExcludeFromCodeCoverage]
public class HttpStatusTests
{
    [Fact]
    public void ShouldReturn500WhenAnErrorOccurs()
    {
        var mediator = new TestProvider().GetService<Mediator>();
        mediator.AddNotification("Test Notification", DomainNotificationType.SystemError);
        Assert.Equal(500, mediator.GetHttpStatusCode());
    }

    [Fact]
    public void ShouldReturn400WhenThereIsAnInvalidRequest()
    {
        var mediator = new TestProvider().Mediator;
        mediator.AddNotification("Test Notification", DomainNotificationType.BadRequest);
        Assert.Equal(400, mediator.GetHttpStatusCode());
    }

    [Fact]
    public void ShouldReturn403WhenThereIsAnForbiddenResponse()
    {
        var mediator = new TestProvider().Mediator;
        mediator.AddNotification("Test Notification", DomainNotificationType.Forbidden);
        Assert.Equal(403, mediator.GetHttpStatusCode());
    }

    [Fact]
    public void ShouldReturn404WhenAResourceIsNotFound()
    {
        var mediator = new TestProvider().Mediator;
        mediator.AddNotification("Test Notification", DomainNotificationType.NotFound);
        Assert.Equal(404, mediator.GetHttpStatusCode());
    }

    [Fact]
    public void ShouldReturn201WhenAResourceIsCreated()
    {
        var mediator = new TestProvider().Mediator;
        mediator.AddNotification("Test Notification", DomainNotificationType.SuccessfullyCreated);
        Assert.Equal(201, mediator.GetHttpStatusCode());
    }

    [Fact]
    public void ShouldReturn200WhenThereIsAResponse()
    {
        var mediator = new TestProvider().Mediator;
        Assert.Equal(200, mediator.GetHttpStatusCode());
    }
}