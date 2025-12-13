using Evently.Shared.Domain;

namespace Evently.Modules.Events.Domain.TicketType;

public static class TicketTypeErrors
{
    public static Error NotFound(Guid eventId) =>
        Error.NotFound("TicketType.NotFound", $"TicketType with id: {eventId} not found.");
}
