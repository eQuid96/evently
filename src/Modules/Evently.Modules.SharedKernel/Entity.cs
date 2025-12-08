namespace Evently.Modules.SharedKernel;

public abstract class Entity
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList();
    private readonly List<IDomainEvent> _domainEvents = [];
    
    protected void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
