using CuentaService.Dominio;

namespace CuentaService.Aplicacion.Dtos;

public record MovimientoDto(int Id, int CuentaId, DateTime Fecha, string TipoMovimiento, decimal Valor, decimal Saldo)
{
    public static MovimientoDto DesdeEntidad(Movimiento movimiento) => new(
        movimiento.Id,
        movimiento.CuentaId,
        movimiento.Fecha,
        movimiento.TipoMovimiento.ToString(),
        movimiento.Valor,
        movimiento.Saldo);
}
