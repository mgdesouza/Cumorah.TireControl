using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TireControl.Domain.Entities;

namespace TireControl.Infrastructure.Data.Configurations;

public class UsuarioRoleConfiguration : IEntityTypeConfiguration<UsuarioRole>
{
    public void Configure(EntityTypeBuilder<UsuarioRole> builder)
    {
        builder.ToTable("UsuarioRoles");

        builder.HasKey(x => new { x.UsuarioId, x.RoleId });
    }
}
