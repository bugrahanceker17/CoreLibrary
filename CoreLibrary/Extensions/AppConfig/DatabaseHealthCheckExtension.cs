using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CoreLibrary.Extensions.AppConfig;

public static class DatabaseHealthCheckExtension
{
    public static void DbHealthCheckConfigure(this IServiceCollection services, string connectionString)
    {
        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: connectionString,
                    healthQuery: "SELECT 1",
                    name: "MS SQL Server Check",
                    failureStatus: HealthStatus.Unhealthy | HealthStatus.Degraded,
                    tags: new string[] { "db", "sql", "sqlserver" });   
        }
    }

    public static void DbHealthCheckRegister(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}