using Evently.Modules.SharedKernel;

namespace Evently.Modules.Events.Domain.Events;

public static class EventErrors
{
    public static readonly Error EndDateBeforeStartDate = 
        Error.Problem("Events.EndDateBeforeStartDate","An events can't end before start date,");
    
    public static readonly Error StartDateInPast = 
        Error.Problem("Events.StartDateInPast", "The event start date is in the past,");

    public static readonly Error NotDraft = 
        Error.Problem("Events.NotDraft", "The event is not in draft status");

    public static readonly Error CancelCompletedEvent =
        Error.Problem("Events.CancelACompletedEvent", "Unable to cancel an already completed event.");
    
    public static readonly Error CancelStartedEvent =
        Error.Problem("Events.CancelStartedEvent", "Unable to cancel an already started event.");

    public static readonly Error AlreadyCanceled =
        Error.Problem("Events.AlreadyCanceled", "The event has already been canceled.");
    
    public static Error NotFound(Guid eventId) =>
        Error.NotFound("Events.NotFound", $"Event with id: {eventId} not found.");
}
