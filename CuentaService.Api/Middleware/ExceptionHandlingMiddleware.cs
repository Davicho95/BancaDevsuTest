using System.Net;
using System.Text.Json;
using CuentaService.Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace CuentaService.Api.Middleware;

// Un solo lugar para traducir excepciones de negocio a HTTP. SaldoNoDisponibleException cae
// aca como cualquier otra DomainException y sale como 400 con su mensaje tal cual.
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex) when (ex is INotFoundException)
        {
            await EscribirAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (DomainException ex)
        {
            await EscribirAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado procesando {Path}", context.Request.Path);
            await EscribirAsync(context, HttpStatusCode.InternalServerError, "Ocurrio un error inesperado.");
        }
    }

    private static Task EscribirAsync(HttpContext context, HttpStatusCode statusCode, string detail)
    {
        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = statusCode.ToString(),
            Detail = detail
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problemDetails.Status!.Value;
        return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}
