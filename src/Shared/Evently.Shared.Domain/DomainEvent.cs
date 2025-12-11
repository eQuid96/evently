namespace Evently.Shared.Domain;

public abstract class DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccuredOnUtc = DateTime.UtcNow;
    }

    protected DomainEvent(Guid id, DateTime occurredOnUtc)
    {
        Id = id;
        OccuredOnUtc = occurredOnUtc;
    }
    
    public Guid Id { get; init; }
    public DateTime OccuredOnUtc { get; init; }
}
