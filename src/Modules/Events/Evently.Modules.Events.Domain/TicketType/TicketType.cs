using Evently.Modules.Events.Domain.Events;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Domain.TicketType;

public class TicketType : Entity
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; }
    public decimal Quantity { get; private set; }
    
    private TicketType()
    {
        
    }

    public static TicketType Create(
        Event @event, 
        string name, 
        decimal price,
        string currency,
        decimal quantity)
    {

        var ticket = new TicketType
        {
            Id = Guid.NewGuid(),
            EventId = @event.Id,
            Name = name,
            Currency = currency,
            Quantity = quantity,
            Price = price
        };

        ticket.RaiseEvent(new TicketTypeCreatedDomainEvent(ticket.Id));
        return ticket;
    }
}
