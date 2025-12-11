using Evently.Modules.Events.Application.Events;
using Evently.Shared.Domain;
using Evently.Shared.Presentation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal sealed class GetEvents : IEndPoint
{
    public void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapGet("events", async ([FromServices] ISender sender, CancellationToken token) =>
            {
                Result<IReadOnlyCollection<EventResponse>> response = await sender.Send(new QueryGetEvents(), token);
                return response.Match(Results.Ok, ApiResults.ToProblemDetail);
            })
            .WithTags(Tags.Events);
    }
}
