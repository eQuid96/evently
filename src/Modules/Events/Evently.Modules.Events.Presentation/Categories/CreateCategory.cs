using Evently.Modules.Events.Application.Categories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal static class CreateCategory
{
    internal sealed class Request
    {
        public string Name { get; init; }
    }
    
    public static void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", async (
            [FromBody] Request request,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            Guid response = await sender.Send(new CommandCreateCategory(request.Name), cancellationToken);
            return Results.Ok(response);
        }).WithTags(Tags.Categories);
    }
}
