using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TireControl.Domain.Entities;

namespace TireControl.Api.Authentication;

public sealed class JwtTokenService(JwtSettings settings) : ITokenService
{
    public AuthToken Create(Usuario usuario, IReadOnlyCollection<string> roles, IReadOnlyCollection<string> permissions)
    {
        var now = DateTime.UtcNow;
        var expiresAt = now.AddMinutes(settings.ExpirationMinutes);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Email, usuario.Email)
        };

        if (usuario.ClienteId is Guid clienteId)
        {
            claims.Add(new Claim(CustomClaimTypes.ClienteId, clienteId.ToString()));
        }

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        claims.AddRange(permissions.Select(permission => new Claim(CustomClaimTypes.Permission, permission)));

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            notBefore: now,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AuthToken(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
