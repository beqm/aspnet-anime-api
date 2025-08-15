using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logger()
    .Database(builder.Configuration)
    .AutoMapper()
    .Mediatr()
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
app.UseApiVersioning();

app.Run();