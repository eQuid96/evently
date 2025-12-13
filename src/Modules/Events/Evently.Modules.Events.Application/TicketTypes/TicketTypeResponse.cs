namespace Evently.Modules.Events.Application.TicketTypes;

public record TicketTypeResponse(
    Guid TicketTypeId,
    Guid EventId,
    string Name,
    decimal Price,
    decimal Quantity,
    string Currency);
