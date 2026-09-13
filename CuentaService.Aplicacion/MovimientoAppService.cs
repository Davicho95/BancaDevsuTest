using CuentaService.Aplicacion.Dtos;
using CuentaService.Aplicacion.Excepciones;
using CuentaService.Dominio;
using CuentaService.Dominio.Excepciones;
using CuentaService.Dominio.Repositorios;

namespace CuentaService.Aplicacion;

public class MovimientoAppService : IMovimientoAppService
{
    private readonly ICuentaRepository _cuentaRepository;

    public MovimientoAppService(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
    }

    public async Task<MovimientoDto> RegistrarAsync(RegistrarMovimientoRequest request, CancellationToken cancellationToken = default)
    {
        var cuenta = await _cuentaRepository.ObtenerPorNumeroCuentaAsync(request.NumeroCuenta, cancellationToken)
            ?? throw new CuentaNoEncontradaException(request.NumeroCuenta);

        if (!Enum.TryParse<TipoMovimiento>(request.TipoMovimiento, ignoreCase: true, out var tipo))
            throw new MovimientoInvalidoException($"Tipo de movimiento invalido: '{request.TipoMovimiento}'. Use Deposito o Retiro.");

        // Cuenta valida el saldo y se actualiza sola, aca no hay logica de negocio.
        var movimiento = cuenta.RegistrarMovimiento(tipo, request.Valor);
        await _cuentaRepository.ActualizarAsync(cuenta, cancellationToken);

        return MovimientoDto.DesdeEntidad(movimiento);
    }

    public async Task<IReadOnlyList<MovimientoDto>> ObtenerPorCuentaAsync(int cuentaId, CancellationToken cancellationToken = default)
    {
        var cuenta = await _cuentaRepository.ObtenerPorIdAsync(cuentaId, cancellationToken)
            ?? throw new CuentaNoEncontradaException(cuentaId);

        return cuenta.Movimientos.OrderBy(m => m.Fecha).Select(MovimientoDto.DesdeEntidad).ToList();
    }
}
