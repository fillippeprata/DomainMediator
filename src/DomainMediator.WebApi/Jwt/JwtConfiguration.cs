#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace DomainMediator.WebApi.Jwt;

internal interface IJwtConfiguration
{
    public string Issuer { get; }
    public string Audience { get; }
    public string? Key { get; }
    public int ExpirationInMinutes { get; }
}

internal class JwtConfigurationImp() : IJwtConfiguration
{
    public JwtConfigurationImp(IConfiguration configuration) : this()
    {
        var options = configuration.GetSection("JwtConfiguration");
        var configurations = options.Get<JwtConfigurationImp>();
        if (configurations == null)
            return;

        Issuer = configurations.Issuer;
        Audience = configurations.Audience;
        Key = configurations.Key;
        ExpirationInMinutes = configurations.ExpirationInMinutes;
    }

    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string? Key { get; init; }
    public int ExpirationInMinutes { get; init; }
}
