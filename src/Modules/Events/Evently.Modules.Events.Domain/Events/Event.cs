using Evently.Modules.SharedKernel;

namespace Evently.Modules.Events.Domain.Events;

public sealed class Event : Entity
{
    private Event()
    {
        
    }
    
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Location { get; private set; }
    public DateTime StartsAtUtc { get; private set; }
    public DateTime? EndsAtUtc { get; private set; }
    public EventStatus EventStatus { get; private set; }
    public Guid CategoryId { get; private set; }
    public static Event Create(
        string title,
        string description,
        string location,
        DateTime startAtsUtc,
        Guid categoryId,
        DateTime? endsAtUtc)
    {
        if (endsAtUtc.HasValue && endsAtUtc < startAtsUtc)
        {
            throw new InvalidOperationException(EventsErrors.EndDateBeforeStartDate);
        }
        
        var @event = new Event
        {
            Id = Guid.NewGuid(),
            Title = title,
            Location = location,
            Description = description,
            StartsAtUtc = startAtsUtc,
            EndsAtUtc = endsAtUtc,
            EventStatus = EventStatus.Draft,
            CategoryId = categoryId
        };
        
        @event.RaiseEvent(new EventCreatedDomainEvent(@event.Id));
        
        return @event;
    }
}
