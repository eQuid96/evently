using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Infrastructure.Categories;
using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Shared.Application.Data;
using Evently.Shared.Presentation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{

    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        string dbConnectionString = configuration.GetConnectionString("EventsDatabase");
        
        services.AddDbContext<EventsDbContext>(options => 
            options.UseNpgsql(dbConnectionString,
                    npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.Events)
                )
                .UseSnakeCaseNamingConvention()
        );
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        
        //Register all EndPoints to DI
        services.AddEndPoints(Presentation.AssemblyReference.Assembly);
        return services;
    }
}
