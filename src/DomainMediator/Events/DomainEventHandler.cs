using MediatR;

namespace DomainMediator.Events;

public abstract class DomainEventHandler<EventT> : INotificationHandler<EventT> where EventT : IDomainEvent
{
    public async Task Handle(EventT notification, CancellationToken cancellationToken)
    {
        await Handle(notification);
    }

    public abstract Task Handle(EventT domainEvent);
}