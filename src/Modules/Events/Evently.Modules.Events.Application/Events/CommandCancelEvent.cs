using Evently.Modules.Events.Domain.Events;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Application.Time;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.Events;

public sealed record CommandCancelEvent(Guid EventId) : ICommand;


internal sealed class CommandHandlerCancelEvent(
    IEventRepository eventRepository, 
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) 
    : ICommandHandler<CommandCancelEvent>
{
    public async Task<Result> Handle(CommandCancelEvent request, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(request.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure(EventErrors.NotFound(request.EventId));
        }

        Result result = @event.Cancel(dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}
