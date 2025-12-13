namespace Evently.Modules.Events.Domain.TicketType;

public interface ITicketTypeRepository
{
    Task<TicketType?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    void Insert(TicketType ticketType);
}
