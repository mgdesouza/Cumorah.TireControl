using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TireControl.Infrastructure.Data;

namespace TireControl.Api.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Get connection string (fallback to LocalDB sample if not set)
        var connectionString = configuration.GetConnectionString("Default")
                               ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TireControlDb;Integrated Security=True;";

        services.AddDbContext<TireControlDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}
