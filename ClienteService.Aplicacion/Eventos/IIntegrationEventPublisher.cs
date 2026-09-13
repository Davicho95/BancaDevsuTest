namespace ClienteService.Aplicacion.Eventos;

// Puerto hacia el broker. Aplicacion no deberia enterarse de que hay RabbitMQ/MassTransit debajo.
public interface IIntegrationEventPublisher
{
    Task PublishAsync<TEvent>(TEvent evento, CancellationToken cancellationToken = default) where TEvent : class;
}
