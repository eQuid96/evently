using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketType;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.TicketTypes;

public record CommandCreateTicketType(
    Guid EventId,
    string Name,
    decimal Price,
    decimal Quantity,
    string Currency) : ICommand<TicketTypeResponse>;


internal sealed class CommandHandlerCreateTicketType(
    IEventRepository eventRepository,
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CommandCreateTicketType, TicketTypeResponse>
{
    public async Task<Result<TicketTypeResponse>> Handle(CommandCreateTicketType request, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(request.EventId, cancellationToken);
        if (@event is null)
        {
            return Result.Failure<TicketTypeResponse>(EventErrors.NotFound(request.EventId));
        }

        var ticketType = TicketType.Create(@event, request.Name, request.Price, request.Currency, request.Quantity);
        
        ticketTypeRepository.Insert(ticketType);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new TicketTypeResponse(
            ticketType.Id,
            @event.Id,
            ticketType.Name,
            ticketType.Price,
            ticketType.Quantity,
            ticketType.Currency);

        return Result<TicketTypeResponse>.Ok(response);
    }
}
