using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MedVet.Api.Health;

public static class HealthCheckResponseWriter
{
    public static Task WriteJsonResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var isDevelopment = context.RequestServices.GetService<IHostEnvironment>()?.IsDevelopment() ?? false;

        var payload = new
        {
            status = report.Status.ToString(),
            duration = report.TotalDuration,
            checks = report.Entries.Select(e =>
            {
                var desc = e.Value.Description ?? GetDefaultDescription(e.Key, e.Value.Status);
                return new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = desc,
                    duration = e.Value.Duration,
                    error = isDevelopment ? (e.Value.Exception?.Message ?? (e.Value.Status != HealthStatus.Healthy ? desc : null)) : null
                };
            })
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }

    private static string GetDefaultDescription(string key, HealthStatus status)
    {
        return key.ToLowerInvariant() switch
        {
            "self" => status == HealthStatus.Healthy
                ? "Servico da API ativo e operacional."
                : "Servico da API com instabilidade.",
            "database" => status == HealthStatus.Healthy
                ? "Conexao com o banco de dados estabelecida com sucesso."
                : "Falha na conexao com o banco de dados.",
            "fiap" => status == HealthStatus.Healthy
                ? "Conectividade externa com portal FIAP verificada."
                : "Falha de conectividade com o portal FIAP.",
            _ => status.ToString()
        };
    }
}
