using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace Evently.Shared.Infrastructure.HealthChecks;

internal sealed class NpgsqlHealthCheck(string connectionString) : IHealthCheck
{
    public const string Name = "Npgsql";
    private const string HealthCheckQuery = "SELECT 1;";

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            await using var npgsqlConnection = new NpgsqlConnection(connectionString);

            await npgsqlConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

            await using NpgsqlCommand command = npgsqlConnection.CreateCommand();
            command.CommandText = HealthCheckQuery;
            
            await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);

            return HealthCheckResult.Healthy();
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(description: exception.Message, exception);
        }
    }
}
