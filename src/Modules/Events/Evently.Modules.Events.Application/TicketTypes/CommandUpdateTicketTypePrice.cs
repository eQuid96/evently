using Evently.Modules.Events.Domain.TicketType;
using Evently.Shared.Application.Communication;
using Evently.Shared.Application.Data;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.TicketTypes;

public sealed record CommandUpdateTicketTypePrice(Guid TicketTypeId, decimal NewPrice) 
    : ICommand<TicketTypeResponse>;


internal sealed class CommandHandlerUpdateTicketTypePrice(
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CommandUpdateTicketTypePrice, TicketTypeResponse>
{
    public async Task<Result<TicketTypeResponse>> Handle(CommandUpdateTicketTypePrice request, CancellationToken cancellationToken)
    {
        TicketType? ticketType = await ticketTypeRepository.GetAsync(request.TicketTypeId, cancellationToken);
        if (ticketType is null)
        {
            return Result.Failure<TicketTypeResponse>(TicketTypeErrors.NotFound(request.TicketTypeId));
        }
        
        ticketType.UpdatePrice(request.NewPrice);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var response = new TicketTypeResponse(
            ticketType.Id,
            ticketType.EventId,
            ticketType.Name,
            ticketType.Price,
            ticketType.Quantity,
            ticketType.Currency);

        return Result<TicketTypeResponse>.Ok(response);
    }
}
