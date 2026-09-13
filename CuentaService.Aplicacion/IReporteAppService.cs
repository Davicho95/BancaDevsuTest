using CuentaService.Aplicacion.Dtos;

namespace CuentaService.Aplicacion;

public interface IReporteAppService
{
    Task<IReadOnlyList<MovimientoReporteDto>> GenerarEstadoCuentaAsync(
        string clienteIdentificacion,
        DateTime desde,
        DateTime hasta,
        CancellationToken cancellationToken = default);
}
