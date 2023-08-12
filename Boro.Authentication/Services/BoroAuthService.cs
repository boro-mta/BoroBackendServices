using Boro.Authentication.Extensions;
using Boro.Authentication.Interfaces;
using Boro.Authentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Boro.Authentication.Services;

internal class BoroAuthService : IBoroAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ISendBirdIdentityBackend _sendBird;
    private readonly SigningCredentials _signingCredentials;

    public BoroAuthService(JwtSettings jwtSettings,
                           ISendBirdIdentityBackend sendBird)
    {
        _jwtSettings = jwtSettings;
        _sendBird = sendBird;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }

    public TokenDetails GenerateJwtToken(Guid userId, params (AdditionalClaims claim, string? value)[] additionalClaims)
    {
        var sendBirdUser = _sendBird.GetSendBirdUserAsync(userId).Result;
        var sendBirdClaims = sendBirdUser.ToJwtClaims();

        IEnumerable<Claim> defaultClaims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var additionalClaimsQ = additionalClaims
            .Where(pair => !pair.value.IsNullOrEmpty())
            .Select(CreateClaim);

        var claims = Enumerable.Empty<Claim>()
                               .Concat(defaultClaims)
                               .Concat(sendBirdClaims)
                               .Concat(additionalClaimsQ);

        var expirationTime = DateTime.UtcNow.AddDays(1);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims.ToList(),
            notBefore: DateTime.UtcNow,
            expires: expirationTime,
            signingCredentials: _signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return new TokenDetails(tokenHandler.WriteToken(token));
    }

    private static Claim CreateClaim((AdditionalClaims claim, string? value) pair) => pair.claim switch
    {
        AdditionalClaims.FullName => new Claim(JwtRegisteredClaimNames.Name, pair.value!),
        AdditionalClaims.Email => new Claim(JwtRegisteredClaimNames.Email, pair.value!),
        AdditionalClaims.FacebookId => new Claim("fbid", pair.value!),
        _ => throw new NotImplementedException($"No implemented claim for {pair.claim}"),
    };
}

