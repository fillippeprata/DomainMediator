using System.Diagnostics.CodeAnalysis;
using DomainMediator.Commands;
using DomainMediator.ExampleContext.Domain.Services;
using DomainMediator.WebApi.Jwt;
using FluentValidation;

namespace DomainMediator.WebApi.Example.Features.Auth;

[ExcludeFromCodeCoverage]
public record AuthenticateUserCommand : IDomainCommand<JwtResponse>
{
    public string? UserName { get; init; }
    public string? Password { get; init; }
}

public class AuthenticateUserValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class AddUserCommandHandler(IAuthenticationService _authService, IJwtService _jwtService)
    : DomainCommandHandler<AuthenticateUserCommand, JwtResponse>
{
    public override async Task<JwtResponse?> Handle(AuthenticateUserCommand command)
    {
        JwtResponse? jwtResponse = null;

        var user = await _authService.AuthenticateAsync(command.UserName!, command.Password!);

        if (user != null)
            jwtResponse = _jwtService.GenerateAccessToken(user.Id, user.UserName, user.Roles);

        return jwtResponse;
    }
}