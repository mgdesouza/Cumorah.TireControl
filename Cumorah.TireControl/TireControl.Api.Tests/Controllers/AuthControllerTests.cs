using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TireControl.Api.Authentication;
using TireControl.Api.Contracts.Auth;
using TireControl.Api.Controllers;
using TireControl.Domain.Entities;
using TireControl.Domain.Enums;
using TireControl.Infrastructure.Data;

namespace TireControl.Api.Tests.Controllers;

public sealed class AuthControllerTests
{
    [Fact]
    public async Task Login_WithValidCredentials_ReturnsAccessTokenAndUserAuthorizationData()
    {
        await using var dbContext = CreateDbContext();
        var passwordHasher = new PasswordHasher<Usuario>();
        var usuario = CreateActiveUser(passwordHasher, "usuario@exemplo.com", "SenhaSegura1!");
        var permission = new Permission { Id = Guid.NewGuid(), Nome = "Pneus.Ler" };
        var role = new Role { Id = Guid.NewGuid(), Nome = "Administrador" };

        role.RolePermissions.Add(new RolePermission
        {
            Role = role,
            RoleId = role.Id,
            Permission = permission,
            PermissionId = permission.Id
        });
        usuario.UsuarioRoles.Add(new UsuarioRole
        {
            Usuario = usuario,
            UsuarioId = usuario.Id,
            Role = role,
            RoleId = role.Id
        });
        dbContext.Usuarios.Add(usuario);
        await dbContext.SaveChangesAsync();

        var tokenService = new RecordingTokenService();
        var controller = new AuthController(dbContext, passwordHasher, tokenService);

        var result = await controller.Login(
            new LoginRequest { Email = " USUARIO@EXEMPLO.COM ", Password = "SenhaSegura1!" },
            CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<LoginResponse>(okResult.Value);
        Assert.Equal("access-token", response.AccessToken);
        Assert.Equal("Bearer", response.TokenType);
        Assert.Equal(tokenService.Token.ExpiresAtUtc, response.ExpiresAtUtc);
        Assert.Same(usuario, tokenService.Usuario);
        Assert.Equal(["Administrador"], tokenService.Roles);
        Assert.Equal(["Pneus.Ler"], tokenService.Permissions);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorizedWithoutCreatingToken()
    {
        await using var dbContext = CreateDbContext();
        var passwordHasher = new PasswordHasher<Usuario>();
        dbContext.Usuarios.Add(CreateActiveUser(passwordHasher, "usuario@exemplo.com", "SenhaSegura1!"));
        await dbContext.SaveChangesAsync();

        var tokenService = new RecordingTokenService();
        var controller = new AuthController(dbContext, passwordHasher, tokenService);

        var result = await controller.Login(
            new LoginRequest { Email = "usuario@exemplo.com", Password = "SenhaIncorreta1!" },
            CancellationToken.None);

        Assert.IsType<UnauthorizedResult>(result.Result);
        Assert.Null(tokenService.Usuario);
    }

    private static TireControlDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TireControlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TireControlDbContext(options);
    }

    private static Usuario CreateActiveUser(IPasswordHasher<Usuario> passwordHasher, string email, string password)
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "Usuário de teste",
            Email = email,
            TipoUsuario = TipoUsuario.Sistema,
            Ativo = true
        };
        usuario.PasswordHash = passwordHasher.HashPassword(usuario, password);

        return usuario;
    }

    private sealed class RecordingTokenService : ITokenService
    {
        public AuthToken Token { get; } = new("access-token", new DateTime(2026, 8, 31, 12, 0, 0, DateTimeKind.Utc));
        public Usuario? Usuario { get; private set; }
        public IReadOnlyCollection<string> Roles { get; private set; } = [];
        public IReadOnlyCollection<string> Permissions { get; private set; } = [];

        public AuthToken Create(Usuario usuario, IReadOnlyCollection<string> roles, IReadOnlyCollection<string> permissions)
        {
            Usuario = usuario;
            Roles = roles;
            Permissions = permissions;

            return Token;
        }
    }
}
