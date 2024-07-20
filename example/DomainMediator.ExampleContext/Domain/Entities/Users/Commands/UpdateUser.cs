using System.Diagnostics.CodeAnalysis;
using DomainMediator.Commands;

namespace DomainMediator.ExampleContext.Domain.Entities.Users.Commands;

[ExcludeFromCodeCoverage]
public record UpdateUserCommand : _UserCommandBase, IDomainCommand
{
    internal Guid UserId { get; private set; }
    public void SetUserId(Guid userId) => UserId = userId;
}

public class UpdateUserValidator : UserCommandBaseValidator<UpdateUserCommand>;

internal class UpdateUserCommandHandler(UserFactory _factory) : DomainCommandHandler<UpdateUserCommand>
{
    public override async Task Handle(UpdateUserCommand command)
    {
        var user = await _factory.FindUserAsync(command.UserId);
        if (user == null) return;
        await user.UpdateAndSaveAsync(command);
    }
}
