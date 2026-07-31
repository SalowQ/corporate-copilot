using CorporateCopilot.Api.Extensions;
using CorporateCopilot.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

ConfigurePortBinding(builder);

builder.Services.AddControllers();
builder.Services.AddCorporateCopilotServices(builder.Configuration);
builder.Services.AddCorporateCopilotSwagger();
builder.Services.AddProblemDetails();

var app = builder.Build();

// CORS precisa rodar cedo e também ser exigido nos endpoints (ASP.NET Core 8).
app.UseCors(ServiceCollectionExtensions.FrontendCorsPolicy);

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CorporateCopilot API v1");
    options.RoutePrefix = "swagger";
});

app.MapControllers()
    .RequireCors(ServiceCollectionExtensions.FrontendCorsPolicy);

app.Run();

static void ConfigurePortBinding(WebApplicationBuilder builder)
{
    var port = Environment.GetEnvironmentVariable("PORT");

    if (!string.IsNullOrWhiteSpace(port))
    {
        builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
        return;
    }

    var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");

    if (string.IsNullOrWhiteSpace(urls))
    {
        builder.WebHost.UseUrls("http://0.0.0.0:8080");
    }
}
