using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealEstateAnalyzer.Application.Common.Behaviors;

namespace RealEstateAnalyzer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(assembly);
        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
