using Evently.Modules.Events.Application.Categories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal static class ArchiveCategory
{
    public static void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapPut("categories/{id:guid}/archive", async(
            Guid id,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            bool archived = await sender.Send(new CommandArchiveCategory(id), cancellationToken);
            return !archived ? Results.NotFound() : Results.Ok();
        }).WithTags(Tags.Categories);
    }
}
