using DomainMediator.Events;
using DomainMediator.ExampleContext.Domain.Entities.Users.Events;
using DomainMediator.Notifications;

namespace DomainMediator.ExampleContext.Domain.Events;

internal class SendEmailAfterUserAddition(Mediator _mediator) : DomainEventHandler<UserAddedEvent>
{
    public override Task Handle(UserAddedEvent domainEvent)
    {
        _mediator.AddNotification($"Sending e-mail for user {domainEvent.User.CallAs}",
            DomainNotificationType.Information);
        return Task.CompletedTask;
    }
}