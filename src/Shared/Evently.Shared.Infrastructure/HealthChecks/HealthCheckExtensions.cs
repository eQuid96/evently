using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Evently.Shared.Infrastructure.HealthChecks;

public static class HealthCheckExtensions
{
    public static IHealthChecksBuilder AddNpgSql(
        this IHealthChecksBuilder builder,
        string connectionString,
        HealthStatus? failureStatus = null,
        IEnumerable<string>? tags = default)
    {
        
        HealthCheckRegistration registration = new(
            NpgsqlHealthCheck.Name,
            services => new NpgsqlHealthCheck(connectionString),
            failureStatus, 
            tags);
        builder.Add(registration);
        return builder;
    }
}
