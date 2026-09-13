using ClienteService.Aplicacion.Eventos;
using MassTransit;

namespace ClienteService.Infraestructura.Mensajeria;

public class MassTransitIntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitIntegrationEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<TEvent>(TEvent evento, CancellationToken cancellationToken = default) where TEvent : class =>
        _publishEndpoint.Publish(evento, cancellationToken);
}
