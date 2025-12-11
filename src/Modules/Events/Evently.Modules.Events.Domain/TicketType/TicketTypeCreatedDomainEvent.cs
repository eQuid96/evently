using Evently.Shared.Domain;

namespace Evently.Modules.Events.Domain.TicketType;

public class TicketTypeCreatedDomainEvent(Guid ticketId) : DomainEvent
{
    public Guid TicketId { get; init; } = ticketId;
}
