using ClienteService.Aplicacion.Eventos;
using ClienteService.Dominio.Repositorios;
using ClienteService.Infraestructura.Mensajeria;
using ClienteService.Infraestructura.Persistencia;
using ClienteService.Infraestructura.Repositorios;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClienteService.Infraestructura;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructura(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ClienteDb")
            ?? throw new InvalidOperationException("No se encontro la cadena de conexion 'ClienteDb'.");

        services.AddDbContext<ClienteDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IIntegrationEventPublisher, MassTransitIntegrationEventPublisher>();

        services.AddMassTransit(x =>
        {
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
            });
        });

        return services;
    }
}
