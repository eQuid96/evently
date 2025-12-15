using Evently.Shared.Application.Cache;
using Evently.Shared.Application.Data;
using Evently.Shared.Application.Time;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using StackExchange.Redis;

namespace Evently.Shared.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddEventlyInfrastructure(
        this IServiceCollection services,
        string dbConnectionString,
        string redisConnectionString)
    {

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(dbConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        try
        {
            IConnectionMultiplexer redisConnectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            services.TryAddSingleton(redisConnectionMultiplexer);

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory = () => Task.FromResult(redisConnectionMultiplexer);
            });
        }
        catch
        {
            services.AddDistributedMemoryCache();
        }
        
        services.AddScoped<ICacheService, CacheService>();
        return services;
    }
}
