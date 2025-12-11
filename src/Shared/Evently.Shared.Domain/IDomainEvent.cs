namespace Evently.Shared.Domain;

public interface IDomainEvent
{
    Guid Id { get; }

    DateTime OccuredOnUtc { get; }

}
