namespace CuentaService.Dominio.Repositorios;

// Solo lectura: los movimientos se escriben unicamente a traves del aggregate root Cuenta.
public interface IMovimientoRepository
{
    Task<IReadOnlyList<Movimiento>> ObtenerPorClienteYRangoAsync(
        string clienteIdentificacion,
        DateTime desde,
        DateTime hasta,
        CancellationToken cancellationToken = default);
}
