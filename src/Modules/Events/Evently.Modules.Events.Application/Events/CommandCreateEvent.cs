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
    public async Task<Guid> Handle(CommandCreateEvent request, CancellationToken cancellationToken)
    {
        if (request.StartsAtUtc < dateTimeProvider.UtcNow)
        {
            throw new InvalidOperationException(EventsErrors.StartDateInPast);
        }
        
        var @event = Event.Create(
            request.Title,
            request.Description,
            request.Location,
            request.StartsAtUtc,
            request.CategoryId,
            request.EndsAtUtc);
        
        eventRepository.Insert(@event);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return @event.Id;
    }
}
