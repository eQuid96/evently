using System.Net;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace Evently.Shared.Infrastructure.HealthChecks;

internal sealed class RedisHealthCheck(string connectionString) : IHealthCheck
{
    public const string Name = "Redis";
    private const string ClusterOkResponse = "cluster_state:ok";
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await using ConnectionMultiplexer connection = await ConnectionMultiplexer
                .ConnectAsync(connectionString)
                .WaitAsync(TimeSpan.FromSeconds(4), cancellationToken);
            
            foreach (EndPoint endPoint in connection.GetEndPoints(configuredOnly: true))
            {
                IServer server = connection.GetServer(endPoint);

                if (server.ServerType != ServerType.Cluster)
                {
                    await connection.GetDatabase().PingAsync().ConfigureAwait(false);
                    await server.PingAsync().ConfigureAwait(false);
                }
                else
                {
                    RedisResult result = await server.ExecuteAsync("CLUSTER", "INFO").ConfigureAwait(false);

                    if (result.IsNull)
                    {
                        return HealthCheckResult.Unhealthy("Unable to get redis cluster information");
                    }
                    
                    if (!result.ToString().Contains(ClusterOkResponse))
                    {
                        return HealthCheckResult.Unhealthy(
                            description: $"Redis cluster status not ok for endpoint: {endPoint}");
                    }

                }
            }
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(description: exception.Message, exception);
        }
        
        return HealthCheckResult.Healthy();
    }
}
