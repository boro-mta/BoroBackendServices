using Boro.Authentication.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Boro.Authentication.Services;

internal class BoroAuthService : IBoroAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly SigningCredentials _signingCredentials;

    public BoroAuthService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }


    public string GenerateJwtToken(Guid userId, params (AdditionalClaims claim, string value)[] additionalClaims)
    {
        IEnumerable<Claim> claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var q = additionalClaims.Select(pair => pair.claim switch
        {
            AdditionalClaims.FullName => new Claim(JwtRegisteredClaimNames.Name, pair.value),
            AdditionalClaims.Email => new Claim(JwtRegisteredClaimNames.Email, pair.value),
            AdditionalClaims.FacebookId => new Claim("fbid", pair.value),
            _ => throw new NotImplementedException($"No implemented claim for {pair.claim}"),
        });

        claims = claims.Concat(q);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims.ToList(),
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(TokenHandler.DefaultTokenLifetimeInMinutes),
            signingCredentials: _signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}

