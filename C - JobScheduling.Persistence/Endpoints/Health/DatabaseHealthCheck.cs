using JobScheduling.Persistence.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace JobScheduling.Persistence.Endpoints.Health;

internal sealed class DatabaseHealthCheck(ApplicationDbContext dbContext, ILogger<DatabaseHealthCheck> log) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = new())
    {
        log.LogInformation("Checking health of database");

        var canConnect = await dbContext.Database.CanConnectAsync(ct);

        if (!canConnect)
        {
            return HealthCheckResult.Unhealthy("Cannot connect to database.");
        }

        try
        {
            await dbContext.Users.AnyAsync(ct);
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy($"Cannot interact with database: {e.Message}");
        }

        return HealthCheckResult.Healthy("Database is OK!");
    }
}