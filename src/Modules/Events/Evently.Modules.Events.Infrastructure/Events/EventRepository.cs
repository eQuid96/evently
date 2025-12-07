using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Events;

internal sealed class EventRepository(EventsDbContext dbContext) : IEventRepository
{
    public void Insert(Event @event)
    {
        dbContext.Events.Add(@event);
    }

    public Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Events.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
