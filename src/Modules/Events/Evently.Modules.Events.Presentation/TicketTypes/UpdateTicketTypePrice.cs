using Evently.Modules.Events.Application.TicketTypes;
using Evently.Shared.Domain;
using Evently.Shared.Presentation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal sealed class UpdateTicketTypePrice : IEndPoint
{

    internal sealed class Request
    {
        public decimal Price { get; set; }
    }
    
    public void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapPut("ticket-types/{id:guid}/price", async (
            Guid id,
            [FromBody] Request request,
            [FromServices] ISender sender,
            CancellationToken token) =>
        {
            Result<TicketTypeResponse> result = await sender.Send(new CommandUpdateTicketTypePrice(id, request.Price ), token);

            return result.Match(Results.Ok, ApiResults.ToProblemDetail);
        }).WithTags(Tags.TicketTypes);
    }
}
