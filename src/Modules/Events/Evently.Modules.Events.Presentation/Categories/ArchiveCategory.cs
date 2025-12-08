using Evently.Modules.Events.Application.Categories;
using Evently.Modules.SharedKernel;
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
            Result result = await sender.Send(new CommandArchiveCategory(id), cancellationToken);
            
            return result.Match(() => Results.Ok(), ApiResults.ToProblemDetail);
        }).WithTags(Tags.Categories);
    }
}
