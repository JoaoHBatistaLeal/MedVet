using MedVet.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MedVet.Api.Exceptions;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = System.Diagnostics.Activity.Current?.Id ?? httpContext.TraceIdentifier;

        logger.LogError(exception, "Excecao nao tratada: {Message} TraceId: {TraceId}", exception.Message, traceId);

        var (statusCode, title, detail) = MapException(exception, environment);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Type = "about:blank",
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        if (environment.IsDevelopment())
        {
            problem.Extensions["traceId"] = httpContext.TraceIdentifier;
        }

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }

    private static (int StatusCode, string Title, string? Detail) MapException(Exception exception, IHostEnvironment env)
    {
        return exception switch
        {
            ArgumentNullException e => (StatusCodes.Status400BadRequest, "Requisicao invalida", e.Message),
            ArgumentException e => (StatusCodes.Status400BadRequest, "Requisicao invalida", e.Message),
            DomainException e => (StatusCodes.Status400BadRequest, "Erro de dominio", e.Message),
            InvalidOperationException e => (StatusCodes.Status400BadRequest, "Operacao invalida", e.Message),
            KeyNotFoundException e => (StatusCodes.Status404NotFound, "Recurso nao encontrado", e.Message),
            UnauthorizedAccessException e => (StatusCodes.Status401Unauthorized, "Nao autorizado", e.Message),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor", env.IsDevelopment() ? exception.Message : "Ocorreu um erro inesperado. Tente novamente mais tarde.")
        };
    }
}
