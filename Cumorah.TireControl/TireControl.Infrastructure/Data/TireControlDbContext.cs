using Microsoft.EntityFrameworkCore;
using TireControl.Domain.Entities;

namespace TireControl.Infrastructure.Data;
    
public class TireControlDbContext : DbContext
{
    public TireControlDbContext(DbContextOptions<TireControlDbContext> options)
        : base(options)
    {   
    }

    public TireControlDbContext(string connectionString)
        : base(new DbContextOptionsBuilder<TireControlDbContext>()
            .UseSqlServer(connectionString)
            .Options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UsuarioRole> UsuarioRoles => Set<UsuarioRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TireControlDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
