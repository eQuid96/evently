using Evently.Modules.Events.Application.Categories;
using Evently.Shared.Domain;
using Evently.Shared.Presentation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class UpdateCategory : IEndPoint
{
    public void MapEndPoints(IEndpointRouteBuilder app)
    {
        app.MapPut("categories/{id:guid}", async(
            Guid id,
            [FromBody] Request request,
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            Result<CategoryResponse> result = 
                await sender.Send(new CommandUpdateCategory(id, request.Name), cancellationToken);
            
            return result.Match(Results.Ok, ApiResults.ToProblemDetail);
        }).WithTags(Tags.Categories);
    }

    internal sealed class Request
    {
        public string Name { get; set; }
    }
}
