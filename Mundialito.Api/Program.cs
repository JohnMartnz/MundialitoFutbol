using Microsoft.EntityFrameworkCore;
using Mundialito.Api.Endpoints;
using Mundialito.Api.Middleware;
using Mundialito.Api.Serilog;
using Mundialito.Application.Abstractions;
using Mundialito.Application.Abstractions.Data;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Features.Tournaments.Commands.CreateTournament;
using Mundialito.Infrastructure.Idempotency;
using Mundialito.Infrastructure.Persistence;
using Mundialito.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<MundialitoDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    );
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateTournamentCommand).Assembly);
});

// Serilog
SerilogConfig.Configure();
builder.Host.UseSerilog();

// Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IIdempotencyService, IdempotencyService>();
builder.Services.AddScoped<IConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<ITournamentQueryRepository, TournamentQueryRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamQueryRepository, TeamQueryRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerQueryRepository, PlayerQueryRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();

var app = builder.Build();

// Middleware
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// Endpoints
app.MapTournamentEndpoints();
app.MapTeamEndpoints();
app.MapPlayerEndpoints();
app.MapMatchEndpoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<MundialitoDbContext>();

    var retries = 3;
    while (retries > 0)
    {
        try
        {
            context.Database.Migrate();
            logger.LogInformation("Migraciones aplicadas correctamente.");
            break;
        }
        catch (Exception)
        {
            retries--;
            if (retries == 0) throw;
            logger.LogWarning("SQL Server no está listo. Reintentando en 5s... ({Retries} intentos restantes)", retries);
            Thread.Sleep(5000);
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
