using Core.Application.Interfaces;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IPasswordHasher, Infrastructure.Persistence.Authentication.PasswordHasher>();
        services.AddScoped<IJwtProvider, Infrastructure.Persistence.Authentication.JwtProvider>();

        return services;
    }
}
