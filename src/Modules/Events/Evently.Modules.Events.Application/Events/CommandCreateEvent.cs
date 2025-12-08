using Evently.Modules.Events.Domain.Events;
using Evently.Modules.SharedKernel;
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
            return Result<Guid>.Failure(EventsErrors.StartDateInPast);
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
            return Result<Guid>.Failure(@event.GetErrors());
        }
        
        eventRepository.Insert(@event.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return @event.Value.Id;
    }
}
