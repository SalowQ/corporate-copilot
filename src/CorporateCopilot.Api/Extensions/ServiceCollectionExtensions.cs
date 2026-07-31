using CorporateCopilot.Api.Configuration;
using CorporateCopilot.Api.Services;
using CorporateCopilot.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CorporateCopilot.Api.Extensions;

/// <summary>
/// Extensões de registro de serviços da aplicação.
/// </summary>
public static class ServiceCollectionExtensions
{
    public const string FrontendCorsPolicy = "FrontendCors";

    public static IServiceCollection AddCorporateCopilotServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<GeminiOptions>(configuration.GetSection(GeminiOptions.SectionName));
        services.Configure<CorsSettings>(configuration.GetSection(CorsSettings.SectionName));

        services.AddHttpClient<IGeminiService, GeminiService>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<GeminiOptions>>().Value;
            var endpoint = string.IsNullOrWhiteSpace(options.Endpoint)
                ? "https://generativelanguage.googleapis.com/v1beta/"
                : options.Endpoint.TrimEnd('/') + "/";

            client.BaseAddress = new Uri(endpoint);
            client.Timeout = TimeSpan.FromSeconds(60);
            client.DefaultRequestHeaders.Remove("x-goog-api-key");

            if (!string.IsNullOrWhiteSpace(options.ApiKey))
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("x-goog-api-key", options.ApiKey.Trim());
            }
        });

        services.AddSingleton<IKnowledgeBaseService, KnowledgeBaseService>();
        services.AddSingleton<IPromptBuilderService, PromptBuilderService>();

        services.AddCorporateCopilotCors(configuration);

        return services;
    }

    public static IServiceCollection AddCorporateCopilotCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var corsSettings = configuration
            .GetSection(CorsSettings.SectionName)
            .Get<CorsSettings>() ?? new CorsSettings();

        var origins = corsSettings.AllowedOrigins
            .Where(origin => !string.IsNullOrWhiteSpace(origin))
            .Select(origin => origin.Trim().TrimEnd('/'))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (origins.Length == 0)
        {
            origins =
            [
                "http://localhost:5173",
                "http://127.0.0.1:5173"
            ];
        }

        services.AddCors(options =>
        {
            options.AddPolicy(FrontendCorsPolicy, policy =>
            {
                policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("Content-Disposition");
            });
        });

        return services;
    }

    public static IServiceCollection AddCorporateCopilotSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CorporateCopilot API",
                Version = "v1",
                Description = "Assistente corporativo baseado em documentos internos e Google Gemini."
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }
}
