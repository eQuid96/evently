using Evently.Modules.Events.Application.TicketTypes;
using Evently.Shared.Domain;
using Evently.Shared.Presentation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal sealed class GetTicketType : IEndPoint
{
    
    public void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapGet("ticket-types/{id:guid}", async (
            Guid id,
            [FromServices] ISender sender,
            CancellationToken token) =>
        {
            Result<TicketTypeResponse> result = await sender.Send(new QueryGetTicketType(id), token);
            return result.Match(Results.Ok, ApiResults.ToProblemDetail);
        }).WithTags(Tags.TicketTypes);
    }
}
