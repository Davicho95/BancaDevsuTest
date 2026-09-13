using CuentaService.Dominio.Repositorios;
using CuentaService.Infraestructura.Mensajeria;
using CuentaService.Infraestructura.Persistencia;
using CuentaService.Infraestructura.Repositorios;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CuentaService.Infraestructura;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructura(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CuentaDb")
            ?? throw new InvalidOperationException("No se encontro la cadena de conexion 'CuentaDb'.");

        services.AddDbContext<CuentaDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ICuentaRepository, CuentaRepository>();
        services.AddScoped<IMovimientoRepository, MovimientoRepository>();
        services.AddScoped<IClienteInfoRepository, ClienteInfoRepository>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ClienteActualizadoEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(
                    configuration["RabbitMq:Host"] ?? "localhost",
                    configuration["RabbitMq:VirtualHost"] ?? "/",
                    h =>
                    {
                        h.Username(configuration["RabbitMq:Username"] ?? "guest");
                        h.Password(configuration["RabbitMq:Password"] ?? "guest");
                    });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
