using Evently.Shared.Domain;
using MediatR;

namespace Evently.Shared.Application.Communication;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : class, IDomainEvent;
