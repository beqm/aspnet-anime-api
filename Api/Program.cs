using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logger()
    .Database(builder.Configuration)
    .AutoMapper()
    .Mediatr()
    .RateLimit(permitLimit: 100, window: TimeSpan.FromMinutes(1))
    .Swagger()
    .Versioning()
    .Repositories();

builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Middlewares();
app.AutoMigrations();
app.Swagger();
app.UseRateLimiter();
app.UseApiVersioning();

app.Run();