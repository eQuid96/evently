using Evently.Modules.Events.Application.Events;
using Evently.Shared.Domain;
using Evently.Shared.Presentation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal sealed class CreateEvent : IEndPoint
{

    public void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async (
                [FromBody] Request request,
                [FromServices] ISender sender,
                CancellationToken token) =>
            {
                Result<Guid> response = await sender.Send(new CommandCreateEvent(
                    request.Title,
                    request.Description,
                    request.Location,
                    request.StartsAtUtc,
                    request.CategoryId,
                    request.EndsAtUtc), token);


                return response.Match(Results.Ok, ApiResults.ToProblemDetail);
            })
            .WithTags(Tags.Events);
    }
    
    internal sealed class Request
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartsAtUtc { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime? EndsAtUtc { get; set; }
    }
}
