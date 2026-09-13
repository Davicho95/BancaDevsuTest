using CuentaService.Aplicacion.Dtos;
using CuentaService.Dominio.Repositorios;

namespace CuentaService.Aplicacion;

public class ReporteAppService : IReporteAppService
{
    private readonly IMovimientoRepository _movimientoRepository;
    private readonly IClienteInfoRepository _clienteInfoRepository;

    public ReporteAppService(IMovimientoRepository movimientoRepository, IClienteInfoRepository clienteInfoRepository)
    {
        _movimientoRepository = movimientoRepository;
        _clienteInfoRepository = clienteInfoRepository;
    }

    public async Task<IReadOnlyList<MovimientoReporteDto>> GenerarEstadoCuentaAsync(
        string clienteIdentificacion,
        DateTime desde,
        DateTime hasta,
        CancellationToken cancellationToken = default)
    {
        // Npgsql no acepta Kind=Unspecified (lo que manda el model binder) contra timestamptz.
        // De paso, si "hasta" llega sin hora se extiende al final del dia para no perder ese dia.
        var desdeUtc = DateTime.SpecifyKind(desde, DateTimeKind.Utc);
        var hastaUtc = DateTime.SpecifyKind(hasta == hasta.Date ? hasta.Date.AddDays(1).AddTicks(-1) : hasta, DateTimeKind.Utc);

        var movimientos = await _movimientoRepository.ObtenerPorClienteYRangoAsync(clienteIdentificacion, desdeUtc, hastaUtc, cancellationToken);

        // Si todavia no llego el evento de RabbitMQ (o el cliente no existe), mostramos la identificacion nomas.
        var nombreCliente = await _clienteInfoRepository.ObtenerNombreAsync(clienteIdentificacion, cancellationToken)
            ?? clienteIdentificacion;

        return movimientos
            .Select(m => new MovimientoReporteDto(
                m.Fecha,
                nombreCliente,
                m.Cuenta.NumeroCuenta,
                m.Cuenta.TipoCuenta.ToString(),
                m.Cuenta.SaldoInicial,
                m.Cuenta.Estado,
                m.Valor,
                m.Saldo))
            .ToList();
    }
}
