using Core.Application.Behaviors;
using Core.Application.Mapping;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(ValidationBehavior<,>).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        services.AddValidatorsFromAssembly(typeof(ValidationBehavior<,>).Assembly);
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

        return services;
    }
}
