using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace DomainMediator.WebApi.Jwt;

[ExcludeFromCodeCoverage]
public record JwtRequest
{
    public required Guid userId { get; init; }
    public string? UserName { get; init; }
    public string[]? UserRoles { get; init; }
    public Claim[]? Claims { get; init; }
}
