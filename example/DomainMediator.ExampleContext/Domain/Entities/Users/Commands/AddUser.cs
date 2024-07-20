using System.Diagnostics.CodeAnalysis;
using DomainMediator.Commands;
using FluentValidation;

namespace DomainMediator.ExampleContext.Domain.Entities.Users.Commands;

[ExcludeFromCodeCoverage]
public record AddUserCommand : _UserCommandBase, IDomainCommand<UserResponse>
{
    public string? UserName { get; init; }
}

public class AddUserValidator : UserCommandBaseValidator<AddUserCommand>
{
    public AddUserValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
    }
}

internal class AddUserCommandHandler(UserFactory _factory) : DomainCommandHandler<AddUserCommand, UserResponse>
{
    public override async Task<UserResponse?> Handle(AddUserCommand command)
    {
        return await _factory.AddUserAndSaveAsync(command);
    }
}