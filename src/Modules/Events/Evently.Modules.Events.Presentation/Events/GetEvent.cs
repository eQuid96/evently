using Evently.Modules.Events.Application.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class GetEvent
{
    public static void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}", async (Guid id, [FromServices] ISender sender) =>
            {
                EventResponse? response = await sender.Send(new QueryGetEvent(id));
                return response is null ? Results.NotFound(id) : Results.Ok(response);
            })
            .WithTags(Tags.Events);
    }
}
