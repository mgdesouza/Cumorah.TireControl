using Microsoft.EntityFrameworkCore;

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




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure entity mappings here when domain entities are defined
    }
}
