using Evently.Modules.Events.Application.Categories;
using Evently.Modules.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal static class GetCategories
{
    internal static void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async ([FromServices]ISender sender, CancellationToken token) =>
        {
            Result<IReadOnlyCollection<CategoryResponse>> result = await sender.Send(new QueryGetCategories(), token);
            return result.Match(Results.Ok, ApiResults.ToProblemDetail);
        }).WithTags(Tags.Categories);
    }
}
