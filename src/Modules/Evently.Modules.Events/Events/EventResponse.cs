namespace Evently.Modules.Events.Events;

public sealed record EventResponse(
    Guid Id,
    string Title,
    string Description,
    string Location,
    DateTime StartsAt,
    DateTime? EndsAt);
