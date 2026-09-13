using Microsoft.Extensions.DependencyInjection;

namespace CuentaService.Aplicacion;

public static class DependencyInjection
{
    public static IServiceCollection AddAplicacion(this IServiceCollection services)
    {
        services.AddScoped<ICuentaAppService, CuentaAppService>();
        services.AddScoped<IMovimientoAppService, MovimientoAppService>();
        services.AddScoped<IReporteAppService, ReporteAppService>();

        return services;
    }
}
