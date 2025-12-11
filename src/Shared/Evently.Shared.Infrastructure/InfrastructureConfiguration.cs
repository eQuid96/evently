using Evently.Shared.Application.Data;
using Evently.Shared.Application.Time;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace Evently.Shared.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddEventlyInfrastructure(this IServiceCollection services,
        string dbConnectionString)
    {

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(dbConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
}
