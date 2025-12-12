using Evently.Modules.Events.Domain.Events;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Application.Time;
using Evently.Shared.Domain;


namespace Evently.Modules.Events.Application.Events;

public sealed record CommandCreateEvent(
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    Guid CategoryId,
    DateTime? EndsAtUtc) : ICommand<Guid>;

internal sealed class HandlerCreateEventCommand(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CommandCreateEvent, Guid>
{
    public async Task<Result<Guid>> Handle(CommandCreateEvent request, CancellationToken cancellationToken)
    {
        if (request.StartsAtUtc < dateTimeProvider.UtcNow)
        {
            return Result.Failure<Guid>(EventErrors.StartDateInPast);
        }
        
        Result<Event> @event = Event.Create(
            request.Title,
            request.Description,
            request.Location,
            request.StartsAtUtc,
            request.CategoryId,
            request.EndsAtUtc);

        if (@event.IsFailure)
        {
            return Result.Failure<Guid>(@event.Error);
        }
        
        eventRepository.Insert(@event.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return @event.Value.Id;
    }
}
