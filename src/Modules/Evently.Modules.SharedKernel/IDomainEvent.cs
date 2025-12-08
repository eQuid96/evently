namespace Evently.Modules.SharedKernel;

public interface IDomainEvent
{
    Guid Id { get; }

    DateTime OccuredOnUtc { get; }

}
