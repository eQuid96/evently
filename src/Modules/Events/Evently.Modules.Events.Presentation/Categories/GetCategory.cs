using Evently.Modules.Events.Application.Categories;
using Evently.Modules.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal static class GetCategory
{

    internal static void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapGet("categories/{id:guid}", async (
            Guid id,
            [FromServices] ISender sender,
            CancellationToken token) =>
        {
            Result<CategoryResponse> result = await sender.Send(new QueryGetCategory(id), token);

            return result.Match(Results.Ok, ApiResults.ToProblemDetail);
        }).WithTags(Tags.Categories);
    }
}
