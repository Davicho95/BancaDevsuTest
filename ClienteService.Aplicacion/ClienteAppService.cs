using ClienteService.Aplicacion.Dtos;
using ClienteService.Aplicacion.Eventos;
using ClienteService.Aplicacion.Excepciones;
using ClienteService.Aplicacion.Seguridad;
using ClienteService.Dominio;
using ClienteService.Dominio.Repositorios;
using Shared.Contracts;

namespace ClienteService.Aplicacion;

public class ClienteAppService : IClienteAppService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IIntegrationEventPublisher _eventPublisher;

    public ClienteAppService(
        IClienteRepository clienteRepository,
        IPasswordHasher passwordHasher,
        IIntegrationEventPublisher eventPublisher)
    {
        _clienteRepository = clienteRepository;
        _passwordHasher = passwordHasher;
        _eventPublisher = eventPublisher;
    }

    public async Task<ClienteDto> CrearAsync(CrearClienteRequest request, CancellationToken cancellationToken = default)
    {
        var contrasenaHash = _passwordHasher.Hash(request.Contrasena);
        var cliente = new Cliente(
            request.ClienteId,
            contrasenaHash,
            request.Nombre,
            request.Genero,
            request.Edad,
            request.Identificacion,
            request.Direccion,
            request.Telefono);

        await _clienteRepository.AgregarAsync(cliente, cancellationToken);
        await PublicarActualizacionAsync(cliente, cancellationToken);

        return ClienteDto.DesdeEntidad(cliente);
    }

    public async Task<ClienteDto> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await ObtenerOFallarAsync(id, cancellationToken);
        return ClienteDto.DesdeEntidad(cliente);
    }

    public async Task<IReadOnlyList<ClienteDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var clientes = await _clienteRepository.ObtenerTodosAsync(cancellationToken);
        return clientes.Select(ClienteDto.DesdeEntidad).ToList();
    }

    public async Task<ClienteDto> ActualizarAsync(int id, ActualizarClienteRequest request, CancellationToken cancellationToken = default)
    {
        var cliente = await ObtenerOFallarAsync(id, cancellationToken);

        cliente.ActualizarInformacion(request.Nombre, request.Genero, request.Edad, request.Direccion, request.Telefono);

        await _clienteRepository.ActualizarAsync(cliente, cancellationToken);
        await PublicarActualizacionAsync(cliente, cancellationToken);

        return ClienteDto.DesdeEntidad(cliente);
    }

    public async Task CambiarEstadoAsync(int id, bool activo, CancellationToken cancellationToken = default)
    {
        var cliente = await ObtenerOFallarAsync(id, cancellationToken);

        if (activo)
            cliente.Activar();
        else
            cliente.Desactivar();

        await _clienteRepository.ActualizarAsync(cliente, cancellationToken);
        await PublicarActualizacionAsync(cliente, cancellationToken);
    }

    public async Task EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await ObtenerOFallarAsync(id, cancellationToken);
        await _clienteRepository.EliminarAsync(cliente, cancellationToken);
    }

    private async Task<Cliente> ObtenerOFallarAsync(int id, CancellationToken cancellationToken) =>
        await _clienteRepository.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new ClienteNoEncontradoException(id);

    // Ojo aca: la correlacion entre los dos microservicios es por Identificacion (cedula), no por
    // ClienteId (el login). Asi quedaron sembradas las cuentas en cuentadb, y cambiarlo implicaria migrar datos.
    private Task PublicarActualizacionAsync(Cliente cliente, CancellationToken cancellationToken) =>
        _eventPublisher.PublishAsync(
            new ClienteActualizadoEvent(cliente.Identificacion, cliente.Nombre, cliente.Estado),
            cancellationToken);
}
