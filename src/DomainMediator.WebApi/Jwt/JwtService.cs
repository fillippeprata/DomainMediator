using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DomainMediator.WebApi.Jwt;

public interface IJwtService
{
    JwtResponse GenerateAccessToken(Guid userId, string userName, string[] userRoles, Claim[]? claims = null);
    JwtResponse GenerateAccessToken(JwtRequest request);
}

internal class JwtServiceImp(IJwtConfiguration _jwtConfiguration) : IJwtService
{
    public JwtResponse GenerateAccessToken(Guid userId, string userName, string[]? userRoles = null, Claim[]? claims = null)
    {
        return GenerateAccessToken(new()
        {
            userId = userId,
            UserName = userName,
            UserRoles =  userRoles,
            Claims = claims,
        });
    }

    public JwtResponse GenerateAccessToken(JwtRequest request)
    {
        var claimsList = new List<Claim>
        {
            new(JwtDefaultClaimKeys.userId.ToString(), request.userId.ToString())
        };
        if(request.UserRoles is { Length: > 0 }) claimsList.AddRange(request.UserRoles.Select(role => new Claim(ClaimTypes.Role, role)));
        if (!string.IsNullOrEmpty(request.UserName)) claimsList.Add(new(ClaimTypes.Name, request.UserName));
        if(request.Claims is { Length: > 0 }) claimsList.AddRange(request.Claims);

        var expiresIn = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationInMinutes);
        var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Key!);

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claimsList),
            Expires = expiresIn,
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        });
        var stringToken = tokenHandler.WriteToken(token);

        return new JwtResponse
        {
            UserName = request.UserName,
            UserId = request.userId,
            AccessToken = stringToken,
            ExpirationDateTime = token.ValidTo,
            ExpiresInSeconds = Convert.ToInt64((token.ValidTo - DateTime.UtcNow).TotalSeconds)
        };
    }
}
