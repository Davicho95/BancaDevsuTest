namespace ClienteService.Api;

// Envoltorio solo para respuestas exitosas. Los errores ya tienen su propio formato estandar
// (ProblemDetails, ver ExceptionHandlingMiddleware) y envolverlos tambien seria redundante.
public record ApiResponse<T>(T Data)
{
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}

public static class ApiResponseExtensions
{
    public static ApiResponse<T> ToApiResponse<T>(this T data) => new(data);
}
