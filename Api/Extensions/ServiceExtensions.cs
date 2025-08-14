using MediatR;
using Serilog;
using FluentValidation;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Application.Common.Mappings;
using Application.Common.Behaviors;
using Microsoft.EntityFrameworkCore;
using Application.Commands.Anime.CreateAnime;

namespace Api.Extensions;

public static class ServiceExtensions
{
    public static WebApplicationBuilder Logger(this WebApplicationBuilder builder)
    {
        Serilog.Debugging.SelfLog.Enable(Console.Error);
        var outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u4}] {Message}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.File(
                "../logs/errors.log",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
                outputTemplate: outputTemplate
            )
            .CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }

    public static WebApplicationBuilder Repositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAnimeRepository, AnimeRepository>();
        return builder;
    }

    public static WebApplicationBuilder Database(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return builder;
    }

    public static WebApplicationBuilder AutoMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        return builder;
    }

    public static WebApplicationBuilder Mediatr(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateAnimeCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        builder.Services.AddValidatorsFromAssembly(typeof(CreateAnimeCommand).Assembly);
        return builder;
    }

    public static WebApplicationBuilder Swagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        return builder;
    }

}