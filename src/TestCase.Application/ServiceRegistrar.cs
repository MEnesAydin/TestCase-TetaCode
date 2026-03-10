using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace TestCase.Application;

public static class ServiceRegistrar
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfr =>
        {
            cfr.RegisterServicesFromAssembly(typeof(ServiceRegistrar).Assembly);
        });
        
        services.AddValidatorsFromAssembly(typeof(ServiceRegistrar).Assembly);
        
        return services;
    }
}