namespace JobScheduling.Persistence.Application.Extensions;

public static class ConfigurationExtensions
{
    public static string GetDatabaseConnectionString(this IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("Database");

        return !string.IsNullOrWhiteSpace(conn) 
            ? conn 
            : throw new InvalidOperationException("Expected the `Database` connection string to exist.");
    }
}