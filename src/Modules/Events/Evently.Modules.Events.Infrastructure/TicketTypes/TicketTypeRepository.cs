using Evently.Modules.Events.Domain.TicketType;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.TicketTypes;

internal sealed class TicketTypeRepository(EventsDbContext dbContext) : ITicketTypeRepository
{
    public Task<TicketType?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.TicketTypes.SingleOrDefaultAsync(ticket => ticket.Id == id, cancellationToken);
    }

    public void Insert(TicketType ticketType)
    {
        dbContext.TicketTypes.Add(ticketType);
    }
}
