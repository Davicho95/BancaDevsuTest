using CuentaService.Aplicacion.Dtos;

namespace CuentaService.Aplicacion;

// Solo Crear + Leer: un movimiento contable no se deberia poder editar una vez creado.
public interface IMovimientoAppService
{
    Task<MovimientoDto> RegistrarAsync(RegistrarMovimientoRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MovimientoDto>> ObtenerPorCuentaAsync(int cuentaId, CancellationToken cancellationToken = default);
}
