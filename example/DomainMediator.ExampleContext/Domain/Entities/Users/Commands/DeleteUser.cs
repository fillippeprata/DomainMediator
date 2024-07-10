using System.Diagnostics.CodeAnalysis;
using DomainMediator.Commands;

namespace DomainMediator.ExampleContext.Domain.Entities.Users.Commands;

[ExcludeFromCodeCoverage]
public record DeleteUserCommand : IDomainCommand
{
    internal Guid UserId { get; private set; }
    public void SetUserId(Guid userId) => UserId = userId;
}

public class DeleteUserCommandHandler(UserFactory _factory) : DomainCommandHandler<DeleteUserCommand>
{
    public override async Task Handle(DeleteUserCommand command)
    {
        var user = await _factory.FindUserAsync(command.UserId);
        if (user == null) return;
        await user.DeleteAndSaveAsync(command);
    }
}
