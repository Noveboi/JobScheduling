using JobScheduling.Persistence.Application;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace JobScheduling.Persistence.Endpoints.Health;

internal sealed class DatabaseHealthCheck(ApplicationDbContext dbContext, ILogger<DatabaseHealthCheck> log) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = new())
    {
        log.LogInformation("Checking health of database");

        var canConnect = await dbContext.Database.CanConnectAsync(ct);
        
        return canConnect
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy();
    }
}