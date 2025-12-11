using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Shared.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddEventlyApplication(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddMediatR(mediatr => 
            mediatr.RegisterServicesFromAssemblies(assemblies));

        return services;
    }
}
