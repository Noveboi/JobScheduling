using JobScheduling.Persistence.Endpoints.Health;
using Microsoft.EntityFrameworkCore;

namespace JobScheduling.Persistence.Application.Extensions;

public static class NotImportantDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(configuration.GetDatabaseConnectionString()));
        services.AddHealthChecks().AddCheck<DatabaseHealthCheck>("Database");
        return services;
    }
}