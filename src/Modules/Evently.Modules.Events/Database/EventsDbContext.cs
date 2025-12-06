using Evently.Modules.Events.Events;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Database;

public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options)
{
    internal DbSet<Event> Events { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Events);
    }
}
