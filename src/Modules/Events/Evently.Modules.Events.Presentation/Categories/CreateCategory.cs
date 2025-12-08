using Evently.Modules.Events.Application.Categories;
using Evently.Modules.SharedKernel;
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
            Result<Guid> result = await sender.Send(new CommandCreateCategory(request.Name), cancellationToken);
            
            return result.Match(Results.Ok, ApiResults.ToProblemDetail);
            
        }).WithTags(Tags.Categories);
    }
}
