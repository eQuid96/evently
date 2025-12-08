using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

public static class CategoryEndpoints
{

    public static void MapEndPoints(IEndpointRouteBuilder app)
    {
        CreateCategory.MapEndPoints(app);
        ArchiveCategory.MapEndPoints(app);
    }
}
