namespace Evently.Modules.Events.Domain.Events;

public static class EventsErrors
{
    public const string EndDateBeforeStartDate = "An events can't end before start date";
    public const string StartDateInPast = "The event start date is in the past";

}
