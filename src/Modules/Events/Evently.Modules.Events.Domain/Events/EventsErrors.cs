using Evently.Modules.SharedKernel;

namespace Evently.Modules.Events.Domain.Events;

public static class EventsErrors
{
    public static readonly Error EndDateBeforeStartDate = 
        Error.Problem("Events.EndDateBeforeStartDate","An events can't end before start date,");
    
    public static readonly Error StartDateInPast = 
        Error.Problem("Events.StartDateInPast", "The event start date is in the past,");
    
    public static Error NotFound(Guid eventId) =>
        Error.NotFound("Events.NotFound", $"Event with id: {eventId} not found.");
}
