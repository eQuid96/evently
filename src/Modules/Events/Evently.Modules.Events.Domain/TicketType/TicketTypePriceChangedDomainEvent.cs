using Evently.Shared.Domain;

namespace Evently.Modules.Events.Domain.TicketType;

public class TicketTypePriceChangedDomainEvent(Guid ticketTypeId, decimal newPrice) : DomainEvent
{
    public Guid TicketTypeId { get; init; } = ticketTypeId;
    public decimal Price { get; init; } = newPrice;
}
