namespace TireControl.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
