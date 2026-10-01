using System.Reflection;
using Asp.Versioning;
using MedVet.Api.Exceptions;
using MedVet.Api.Extensions;
using MedVet.Api.Health;
using MedVet.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MedVet.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddMedVetDbContext(builder.Configuration);
        builder.Services.AddMedVetRepositories();
        builder.Services.AddMedVetApplicationServices();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        // API Versioning (CP5: Default 2.0, Query string, Header, ApiExplorer)
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(2, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader("api-version"),
                new HeaderApiVersionReader("X-Api-Version"),
                new UrlSegmentApiVersionReader());
        })
        .AddMvc()
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVVV";
            options.SubstituteApiVersionInUrl = true;
        });

        // Rate Limiting (CP5: Fixed window, 429, Retry-After header)
        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddFixedWindowLimiter("fixed", limiter =>
            {
                limiter.Window = TimeSpan.FromMinutes(1);
                limiter.PermitLimit = 10;
            });
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.Headers.RetryAfter = "60";
                await context.HttpContext.Response.WriteAsJsonAsync(new
                {
                    type = "https://tools.ietf.org/html/rfc6585#section-4",
                    title = "Too Many Requests",
                    status = StatusCodes.Status429TooManyRequests,
                    detail = "Limite de requisicoes excedido. Tente novamente mais tarde."
                }, token);
            };
        });

        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddDbContextCheck<MedVetContext>("database")
            .AddUrlGroup(new Uri("https://fiap.com.br"), "fiap");

        // Swagger configurado dinamicamente para cada versao descoberta
        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }
        });

        var app = builder.Build();

        app.UseExceptionHandler();

        // Rate Limiter middleware apos UseExceptionHandler e antes dos controllers
        app.UseRateLimiter();

        if (app.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("Database:UseSqlite"))
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MedVetContext>();
            db.Database.EnsureCreated();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in app.DescribeApiVersions())
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        $"MedVet API {description.GroupName}");
                }
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        // Health Checks mapeado com isencao de rate limit
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
        }).DisableRateLimiting();

        app.Run();
    }
}