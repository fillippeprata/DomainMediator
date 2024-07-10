using System.Diagnostics.CodeAnalysis;
using DomainMediator.Events;

namespace DomainMediator.ExampleContext.Domain.Entities.Users.Events;

[ExcludeFromCodeCoverage]
public record UserAddedEvent : IDomainEvent
{
    public required IUserProperties User { get; init; }
}
