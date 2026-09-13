using ClienteService.Aplicacion.Dtos;

namespace ClienteService.Aplicacion;

public interface IClienteAppService
{
    Task<ClienteDto> CrearAsync(CrearClienteRequest request, CancellationToken cancellationToken = default);

    Task<ClienteDto> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClienteDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<ClienteDto> ActualizarAsync(int id, ActualizarClienteRequest request, CancellationToken cancellationToken = default);

    Task CambiarEstadoAsync(int id, bool activo, CancellationToken cancellationToken = default);

    Task EliminarAsync(int id, CancellationToken cancellationToken = default);
}
