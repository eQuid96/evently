using System.Reflection;
using Evently.Shared.Application.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Shared.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddEventlyApplication(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddMediatR(mediatr =>
        {
            mediatr.RegisterServicesFromAssemblies(assemblies);
            mediatr.AddOpenBehavior(typeof(RequestLoggerPipelineBehaviour<,>));
        });

        return services;
    }
}
