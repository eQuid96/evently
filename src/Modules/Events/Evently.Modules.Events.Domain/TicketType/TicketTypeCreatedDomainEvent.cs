using Evently.Modules.SharedKernel;

namespace Evently.Modules.Events.Domain.TicketType;

public class TicketTypeCreatedDomainEvent(Guid ticketId) : DomainEvent
{
    public Guid TicketId { get; init; } = ticketId;
}
