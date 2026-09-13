using CuentaService.Dominio.Repositorios;
using MassTransit;
using Shared.Contracts;

namespace CuentaService.Infraestructura.Mensajeria;

public class ClienteActualizadoEventConsumer : IConsumer<ClienteActualizadoEvent>
{
    private readonly IClienteInfoRepository _clienteInfoRepository;

    public ClienteActualizadoEventConsumer(IClienteInfoRepository clienteInfoRepository)
    {
        _clienteInfoRepository = clienteInfoRepository;
    }

    public Task Consume(ConsumeContext<ClienteActualizadoEvent> context)
    {
        var evento = context.Message;
        return _clienteInfoRepository.GuardarOActualizarAsync(
            evento.ClienteIdentificacion,
            evento.Nombre,
            evento.Estado,
            context.CancellationToken);
    }
}
