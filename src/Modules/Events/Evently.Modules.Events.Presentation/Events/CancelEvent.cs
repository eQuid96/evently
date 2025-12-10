using Evently.Modules.Events.Application.Events;
using Evently.Modules.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class CancelEvent
{
    internal static void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapDelete("events/{id:guid}/cancel", async (
            Guid id,
            [FromServices] ISender sender,
            CancellationToken token) =>
        {
            Result result = await sender.Send(new CommandCancelEvent(id), token);
            return result.Match(() => Results.Ok(), ApiResults.ToProblemDetail);
        }).WithTags(Tags.Events);
    }
}
