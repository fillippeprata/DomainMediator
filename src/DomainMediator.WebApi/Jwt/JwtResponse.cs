using DomainMediator.Commands;

namespace DomainMediator.WebApi.Jwt;

public class JwtResponse : ICommandResponse
{
    public Guid UserId { get; init; }
    public string? UserName { get; init; }
    public string? AccessToken { get; init; }
    public DateTimeOffset ExpirationDateTime { get; init; }
    public long ExpiresInSeconds { get; init; }
}
