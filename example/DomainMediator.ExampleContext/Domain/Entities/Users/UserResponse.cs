using System.Diagnostics.CodeAnalysis;
using DomainMediator.Commands;
using DomainMediator.ExampleContext.Domain.Entities.Users.ObjectValues;
using DomainMediator.Queries;

namespace DomainMediator.ExampleContext.Domain.Entities.Users;

[ExcludeFromCodeCoverage]
public record UserResponse : ICommandResponse, IQueryResponse
{
    public Guid UserId { get; init; }
    public string? CallAs { get; init; }
    public string? UserName { get; init; }
    public string[] Roles { get; init; } = [];
    public required NotificationSettings NotificationSettings { get; init; }
}