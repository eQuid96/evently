using Evently.Modules.Events.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Events;

internal static class GetEvent
{
    public static void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}", async (
                Guid id, 
                [FromServices] EventsDbContext dbContext) =>
            {
                EventResponse? eventResponse = await dbContext.Events
                    .AsNoTracking()
                    .Where(e => e.Id == id)
                    .Select(e => new EventResponse(
                        e.Id,
                        e.Title,
                        e.Description,
                        e.Location,
                        e.StartsAtUtc,
                        e.EndsAtUtc))
                    .SingleOrDefaultAsync();

                return eventResponse is null ? Results.NotFound() : Results.Ok(eventResponse);
            })
            .WithTags(Tags.Events);
    }
}
