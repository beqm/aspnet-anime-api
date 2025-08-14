using Api.Middleware;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class AppExtensions
{
    public static WebApplication Middlewares(this WebApplication app)
    {
        app.UseMiddleware<ErrorMiddleware>();
        return app;
    }

    public static WebApplication AutoMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            db.Database.EnsureCreated();

            DatabaseSeeder.SeedAnimeData(db);
        }

        return app;
    }

}