using ClienteService.Aplicacion.Seguridad;
using Microsoft.Extensions.DependencyInjection;

namespace ClienteService.Aplicacion;

public static class DependencyInjection
{
    public static IServiceCollection AddAplicacion(this IServiceCollection services)
    {
        services.AddScoped<IClienteAppService, ClienteAppService>();
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();

        return services;
    }
}
