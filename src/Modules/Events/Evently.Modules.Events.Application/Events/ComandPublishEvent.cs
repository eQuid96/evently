using Evently.Modules.Events.Domain.Events;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;
using FluentValidation;

namespace Evently.Modules.Events.Application.Events;

public sealed record CommandPublishEvent(Guid EventId) : ICommand;

internal sealed class ValidatorCommandPublishEvent : AbstractValidator<CommandPublishEvent>
{
    public ValidatorCommandPublishEvent()
    {
        RuleFor(r => r.EventId).NotEmpty();
    }
}

internal sealed class CommandHandlerPublishEvent(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CommandPublishEvent>
{
    public async Task<Result> Handle(CommandPublishEvent request, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(request.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure(EventErrors.NotFound(request.EventId));
        }

        Result result = @event.Publish();

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}
