using Evently.Modules.Events.Application.Events;
using Evently.Modules.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class PublishEvent
{
    internal static void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapPut("events/{id:guid}/publish", async (
            Guid id,
            [FromServices] ISender sender,
            CancellationToken token) =>
        {
            Result result = await sender.Send(new CommandPublishEvent(id), token);
            return result.Match(() => Results.Ok(), ApiResults.ToProblemDetail);
        }).WithTags(Tags.Events);
    }
}
