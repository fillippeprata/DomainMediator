using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace DomainMediator.ExampleContext.Domain.Entities.Users.Commands;

[ExcludeFromCodeCoverage]
public record _UserCommandBase
{
    public string? CallAs { get; init; }
    public string? Password { get; init; }
    public string[] Roles { get; init; } = [];
}

public class UserCommandBaseValidator<T> : AbstractValidator<T> where T : _UserCommandBase
{
    protected UserCommandBaseValidator()
    {
        RuleFor(x => x.CallAs).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
