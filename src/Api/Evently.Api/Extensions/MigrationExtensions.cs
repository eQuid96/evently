using Evently.Modules.Events.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Api.Extensions;

internal static class MigrationExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        ApplyMigrations<EventsDbContext>(scope);
    }
    
    private static void ApplyMigrations<TDbContext>(IServiceScope scope) where TDbContext : DbContext
    {
        using TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        dbContext.Database.Migrate();
    }
}
