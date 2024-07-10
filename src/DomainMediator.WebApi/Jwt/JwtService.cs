using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DomainMediator.WebApi.Jwt;

public interface IJwtService
{
    JwtResponse GenerateAccessToken(Guid id, string userName, string[] userRoles);
}

internal class JwtServiceImp(IJwtConfiguration _jwtConfiguration) : IJwtService
{
    public JwtResponse GenerateAccessToken(Guid id, string userName, string[] userRoles)
    {
        var claimsList = new List<Claim>
        {
            new(JwtDefaultClaimKeys.userId.ToString(), id.ToString()),
            new(ClaimTypes.Name, userName)
        };
        claimsList.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var expiresIn = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationInMinutes);
        var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Key);

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
            UserName = userName,
            UserId = id,
            AccessToken = stringToken,
            ExpirationDateTime = token.ValidTo,
            ExpiresInSeconds = Convert.ToInt64((token.ValidTo - DateTime.UtcNow).TotalSeconds)
        };
    }
}
