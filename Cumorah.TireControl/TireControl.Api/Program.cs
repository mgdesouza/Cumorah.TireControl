using Microsoft.EntityFrameworkCore;
using TireControl.Api.DependencyInjection;
using TireControl.Api.Middleware;
using TireControl.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Register infrastructure services (DbContext, etc.)
builder.Services.AddInfrastructure(builder.Configuration);
// Add OpenAPI (Swagger)
builder.Services.AddOpenApi();
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
// Health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Apply pending EF Core migrations at startup
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TireControlDbContext>();
    db.Database.Migrate();
}
catch (Exception ex)
{
    // If migration fails, log to console. In production, use proper logging.
    Console.WriteLine($"Database migration failed: {ex.Message}");
}

// Configure the HTTP request pipeline.
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
