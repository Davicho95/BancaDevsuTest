using CuentaService.Aplicacion.Dtos;
using CuentaService.Aplicacion.Excepciones;
using CuentaService.Dominio;
using CuentaService.Dominio.Excepciones;
using CuentaService.Dominio.Repositorios;

namespace CuentaService.Aplicacion;

public class CuentaAppService : ICuentaAppService
{
    private readonly ICuentaRepository _cuentaRepository;

    public CuentaAppService(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
    }

    public async Task<CuentaDto> CrearAsync(CrearCuentaRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<TipoCuenta>(request.TipoCuenta, ignoreCase: true, out var tipo))
            throw new CuentaInvalidaException($"Tipo de cuenta invalido: '{request.TipoCuenta}'. Use Ahorros o Corriente.");

        var cuenta = new Cuenta(request.NumeroCuenta, tipo, request.SaldoInicial, request.ClienteIdentificacion);

        await _cuentaRepository.AgregarAsync(cuenta, cancellationToken);

        return CuentaDto.DesdeEntidad(cuenta);
    }

    public async Task<CuentaDto> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cuenta = await ObtenerOFallarAsync(id, cancellationToken);
        return CuentaDto.DesdeEntidad(cuenta);
    }

    public async Task<IReadOnlyList<CuentaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        var cuentas = await _cuentaRepository.ObtenerTodasAsync(cancellationToken);
        return cuentas.Select(CuentaDto.DesdeEntidad).ToList();
    }

    public async Task<CuentaDto> ActualizarAsync(int id, ActualizarCuentaRequest request, CancellationToken cancellationToken = default)
    {
        var cuenta = await ObtenerOFallarAsync(id, cancellationToken);

        if (request.Estado)
            cuenta.Activar();
        else
            cuenta.Desactivar();

        await _cuentaRepository.ActualizarAsync(cuenta, cancellationToken);

        return CuentaDto.DesdeEntidad(cuenta);
    }

    private async Task<Cuenta> ObtenerOFallarAsync(int id, CancellationToken cancellationToken) =>
        await _cuentaRepository.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new CuentaNoEncontradaException(id);
}
