using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logger()
    .Database(builder.Configuration)
    .AutoMapper()
    .Mediatr()
    .Swagger()
    .Repositories();

builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Middlewares();
app.AutoMigrations();

app.Run();