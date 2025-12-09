using Evently.Modules.Events.Application.Events;
using Evently.Modules.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class GetEvents
{
    internal static void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapGet("events", async ([FromServices] ISender sender, CancellationToken token) =>
            {
                Result<IReadOnlyCollection<EventResponse>> response = await sender.Send(new QueryGetEvents(), token);
                return response.Match(Results.Ok, ApiResults.ToProblemDetail);
            })
            .WithTags(Tags.Events);
    }
}
