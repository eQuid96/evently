using Microsoft.AspNetCore.Routing;

namespace Evently.Shared.Presentation;

public interface IEndPoint
{
    void MapEndPoints(IEndpointRouteBuilder app);
}
