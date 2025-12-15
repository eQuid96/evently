using Evently.Modules.Events.Application.Categories;
using Evently.Shared.Application.Cache;
using Evently.Shared.Domain;
using Evently.Shared.Presentation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class GetCategories : IEndPoint
{
    public void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (
            [FromServices]ISender sender,
            [FromServices] ICacheService cacheService,
            CancellationToken token) =>
        {
            IReadOnlyCollection<CategoryResponse>? cachedCategories = await cacheService
                .GetAsync<IReadOnlyCollection<CategoryResponse>>("categories", token);
            
            if (cachedCategories is not null)
            {
                return Results.Ok(cachedCategories);
            }
            
            Result<IReadOnlyCollection<CategoryResponse>> result = await sender.Send(new QueryGetCategories(), token);

            if (result.IsSuccess)
            {
                await cacheService.SetAsync("categories", result.Value, cancellationToken: token);
            }
            
            return result.Match(Results.Ok, ApiResults.ToProblemDetail);
        }).WithTags(Tags.Categories);
    }
}
