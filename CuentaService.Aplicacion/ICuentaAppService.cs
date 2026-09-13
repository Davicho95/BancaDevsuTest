using CuentaService.Aplicacion.Dtos;

namespace CuentaService.Aplicacion;

// Sin Delete a proposito: una cuenta se desactiva, no se borra.
public interface ICuentaAppService
{
    Task<CuentaDto> CrearAsync(CrearCuentaRequest request, CancellationToken cancellationToken = default);

    Task<CuentaDto> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CuentaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default);

    Task<CuentaDto> ActualizarAsync(int id, ActualizarCuentaRequest request, CancellationToken cancellationToken = default);
}
