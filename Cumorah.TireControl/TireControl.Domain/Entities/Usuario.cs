using TireControl.Domain.Enums;

namespace TireControl.Domain.Entities;

public class Usuario
{
    public Guid Id { get; set; }
    public Guid? ClienteId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public TipoUsuario TipoUsuario { get; set; }
    public bool Ativo { get; set; } = true;

    public ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();
}
