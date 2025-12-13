using System.Reflection;
using Evently.Shared.Application.Logging;
using Evently.Shared.Application.Validation;
using FluentValidation;
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
            mediatr.AddOpenBehavior(typeof(RequestValidationPipelineBehaviour<,>));
        });
        services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);
        return services;
    }
}
