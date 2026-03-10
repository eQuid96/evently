using MediatR;

namespace Evently.Shared.Domain;

public interface IDomainEvent : INotification
{
    Guid Id { get; }

    DateTime OccuredOnUtc { get; }
}
