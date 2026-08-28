using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationExtensions).Assembly;

        // MediatR pipeline
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });


        // FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // Services
        services.AddScoped<IPricingCalculator, PricingCalculator>();

        return services;
    }
}