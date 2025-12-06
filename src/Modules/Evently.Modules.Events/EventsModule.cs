using Evently.Modules.Events.Database;
using Evently.Modules.Events.Events;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events;

public static class EventsModule
{
    public static void MapEndPoints(IEndpointRouteBuilder app)
    {
        CreateEvent.MapEndpoints(app);
        GetEvent.MapEndPoints(app);
    }

    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        string dbConnectionString = configuration.GetConnectionString("EventsDatabase");

        services.AddDbContext<EventsDbContext>(options => 
            options.UseNpgsql(dbConnectionString,
                npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.Events)
                )
                .UseSnakeCaseNamingConvention()
            );
        
        return services;
    }
}
