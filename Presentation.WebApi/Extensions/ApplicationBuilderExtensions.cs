using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Presentation.WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();

        return app;
    }
}
