using Api.Middleware;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Api.Extensions;

public static class AppExtensions
{
    public static WebApplication Middlewares(this WebApplication app)
    {
        app.UseMiddleware<ErrorMiddleware>();
        return app;
    }

    public static WebApplication Swagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });
        }
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