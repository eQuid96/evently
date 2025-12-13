using Evently.Modules.Events.Application.TicketTypes;
using Evently.Shared.Domain;
using Evently.Shared.Presentation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal sealed class CreateTicketType : IEndPoint
{

    internal sealed class Request
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public string Currency { get; set; }
    }
    
    public void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapPost("ticket-types", async (
            [FromBody] Request request,
            [FromServices] ISender sender,
            CancellationToken token) =>
        {
            Result<TicketTypeResponse> result = await sender.Send(new CommandCreateTicketType(
                request.EventId,
                request.Name,
                request.Price,
                request.Quantity,
                request.Currency), token);

            return result.Match(Results.Ok, ApiResults.ToProblemDetail);
        }).WithTags(Tags.TicketTypes);
    }
}
