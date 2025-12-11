using Evently.Modules.Events.Application.Events;
using Evently.Shared.Domain;
using Evently.Shared.Presentation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal sealed class GetEvent : IEndPoint
{
    public void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}", async (Guid id, [FromServices] ISender sender, CancellationToken token) =>
            {
                Result<EventResponse> response = await sender.Send(new QueryGetEvent(id), token);
                return response.Match(Results.Ok, ApiResults.ToProblemDetail);
            })
            .WithTags(Tags.Events);
    }
}
