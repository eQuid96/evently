using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketType;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Infrastructure.TicketTypes;
using Evently.Shared.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Database;

public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<Event> Events { get; set; }
    internal DbSet<Category> Categories { get; set; }
    internal DbSet<TicketType> TicketTypes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Events);

        modelBuilder
            .ApplyConfiguration(new EventEntityConfiguration())
            .ApplyConfiguration(new TicketTypeConfiguration());
    }
}
