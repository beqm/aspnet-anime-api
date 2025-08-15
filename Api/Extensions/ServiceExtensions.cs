using MediatR;
using Serilog;
using FluentValidation;
using System.Reflection;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Infrastructure.Persistence;
using Application.Common.Mappings;
using Application.Common.Behaviors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddSwaggerGen(options =>
        {
            var provider = builder.Services.BuildServiceProvider()
                            .GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo
                    {
                        Title = $"Anime API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString()
                    });
            }

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        return builder;
    }

    public static WebApplicationBuilder Versioning(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;

            // Lendo a versão via URL: api/v1/...
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true; // Substitui a versão no template da rota
        });

        return builder;
    }

}