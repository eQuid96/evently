using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evently.Shared.Presentation;

public static class EndPointExtensions
{
    public static IServiceCollection AddEndPoints(this IServiceCollection services, params Assembly[] assemblies)
    {
        //Register all classes that implement the IEndPoint interface
        IEnumerable<ServiceDescriptor> endPoints = assemblies
            .SelectMany(assembly =>
                assembly.GetTypes()
                    .Where(t => t is { IsAbstract: false, IsInterface: false }
                                && t.IsAssignableTo(typeof(IEndPoint)))
                    .Select(implementation => ServiceDescriptor.Transient(typeof(IEndPoint), implementation))
                    .ToArray()
                );
        
        services.TryAddEnumerable(endPoints);
        return services;
    }

    public static void MapEndPoints(this WebApplication app, RouteGroupBuilder? routeGroup = null)
    {
        IEnumerable<IEndPoint> endPoints = app.Services.GetRequiredService<IEnumerable<IEndPoint>>();

        IEndpointRouteBuilder endpointRouteBuilder = routeGroup is null ? app : routeGroup;

        foreach (IEndPoint endPoint in endPoints)
        {
            endPoint.MapEndPoints(endpointRouteBuilder);
        }
    }
}
