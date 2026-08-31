using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TireControl.Api.Authentication;
using TireControl.Api.Contracts.Auth;
using TireControl.Domain.Entities;
using TireControl.Infrastructure.Data;

namespace TireControl.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    TireControlDbContext dbContext,
    IPasswordHasher<Usuario> passwordHasher,
    ITokenService tokenService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToUpperInvariant();
        var usuario = await dbContext.Usuarios
            .Include(user => user.UsuarioRoles)
                .ThenInclude(usuarioRole => usuarioRole.Role)
                    .ThenInclude(role => role.RolePermissions)
                        .ThenInclude(rolePermission => rolePermission.Permission)
            .SingleOrDefaultAsync(user => user.Email.ToUpper() == email, cancellationToken);

        if (usuario is null || !usuario.Ativo ||
            passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        var roles = usuario.UsuarioRoles.Select(item => item.Role.Nome).ToArray();
        var permissions = usuario.UsuarioRoles
            .SelectMany(item => item.Role.RolePermissions)
            .Select(item => item.Permission.Nome)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        var token = tokenService.Create(usuario, roles, permissions);

        return Ok(new LoginResponse(token.AccessToken, "Bearer", token.ExpiresAtUtc));
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult GetCurrentUser()
    {
        return Ok(new
        {
            Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
            Name = User.Identity?.Name,
            Email = User.FindFirstValue(ClaimTypes.Email),
            ClienteId = User.FindFirstValue(CustomClaimTypes.ClienteId),
            Roles = User.FindAll(ClaimTypes.Role).Select(claim => claim.Value),
            Permissions = User.FindAll(CustomClaimTypes.Permission).Select(claim => claim.Value)
        });
    }
}
