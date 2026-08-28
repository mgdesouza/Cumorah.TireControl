using Microsoft.EntityFrameworkCore;
using TireControl.Domain.Entities;

namespace TireControl.Infrastructure.Data;

public static class AuthorizationSeed
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        var administradorSistemaId = Guid.Parse("10000000-0000-0000-0000-000000000001");
        var suporteId = Guid.Parse("10000000-0000-0000-0000-000000000002");
        var administradorClienteId = Guid.Parse("10000000-0000-0000-0000-000000000003");
        var usuarioClienteId = Guid.Parse("10000000-0000-0000-0000-000000000004");

        var permissions = new[]
        {
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), Nome = "Usuario.View", Descricao = "Visualizar usuários" },
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Nome = "Usuario.Create", Descricao = "Criar usuários" },
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000003"), Nome = "Usuario.Update", Descricao = "Alterar usuários" },
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000004"), Nome = "Usuario.Delete", Descricao = "Excluir usuários" },
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000005"), Nome = "Pneu.View", Descricao = "Visualizar pneus" },
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000006"), Nome = "Pneu.Create", Descricao = "Criar pneus" },
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000007"), Nome = "Pneu.Update", Descricao = "Alterar pneus" },
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000008"), Nome = "Pneu.Delete", Descricao = "Excluir pneus" },
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000009"), Nome = "Veiculo.View", Descricao = "Visualizar veículos" },
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000010"), Nome = "Veiculo.Create", Descricao = "Criar veículos" },
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000011"), Nome = "Veiculo.Update", Descricao = "Alterar veículos" },
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000012"), Nome = "Veiculo.Delete", Descricao = "Excluir veículos" },
            new Permission { Id = Guid.Parse("20000000-0000-0000-0000-000000000013"), Nome = "Relatorio.View", Descricao = "Visualizar relatórios" }
        };

        var roles = new[]
        {
            new Role { Id = administradorSistemaId, Nome = "AdministradorSistema", Descricao = "Acesso administrativo completo ao sistema" },
            new Role { Id = suporteId, Nome = "Suporte", Descricao = "Acesso operacional para suporte aos clientes" },
            new Role { Id = administradorClienteId, Nome = "AdministradorCliente", Descricao = "Administrador da empresa cliente" },
            new Role { Id = usuarioClienteId, Nome = "UsuarioCliente", Descricao = "Usuário padrão da empresa cliente" }
        };

        modelBuilder.Entity<Permission>().HasData(permissions);
        modelBuilder.Entity<Role>().HasData(roles);

        var allPermissionIds = permissions.Select(x => x.Id).ToArray();
        var clientAdminPermissionIds = permissions.Select(x => x.Id).ToArray();
        var clientUserPermissionIds = permissions
            .Where(x => x.Nome is "Pneu.View" or "Veiculo.View" or "Relatorio.View")
            .Select(x => x.Id)
            .ToArray();

        var rolePermissions = new List<RolePermission>();
        AddRolePermissions(rolePermissions, administradorSistemaId, allPermissionIds);
        AddRolePermissions(rolePermissions, suporteId, allPermissionIds);
        AddRolePermissions(rolePermissions, administradorClienteId, clientAdminPermissionIds);
        AddRolePermissions(rolePermissions, usuarioClienteId, clientUserPermissionIds);

        modelBuilder.Entity<RolePermission>().HasData(rolePermissions);
    }

    private static void AddRolePermissions(
        ICollection<RolePermission> target,
        Guid roleId,
        IEnumerable<Guid> permissionIds)
    {
        foreach (var permissionId in permissionIds)
        {
            target.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            });
        }
    }
}
