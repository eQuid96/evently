using Evently.Modules.Events.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Events;

internal static class CreateEvent
{

    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async (
                [FromBody] Request request,
                [FromServices] EventsDbContext dbContext,
                CancellationToken token) =>
            {
                var @event = new Event
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Description = request.Description,
                    Location = request.Location,
                    StartsAtUtc = request.StartsAtUtc,
                    EndsAtUtc = request.EndsAtUtc,
                    EventStatus = EventStatus.Draft
                };

                dbContext.Add(@event);

                await dbContext.SaveChangesAsync(token);

                return Results.Ok(@event.Id);
            })
            .WithTags(Tags.Events);
    }
    
    internal sealed class Request
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartsAtUtc { get; set; }
        public DateTime? EndsAtUtc { get; set; }
    }
}
