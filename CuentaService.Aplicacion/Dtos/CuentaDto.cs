using CuentaService.Dominio;

namespace CuentaService.Aplicacion.Dtos;

public record CuentaDto(
    int Id,
    string NumeroCuenta,
    string TipoCuenta,
    decimal SaldoInicial,
    decimal Saldo,
    bool Estado,
    string ClienteIdentificacion)
{
    public static CuentaDto DesdeEntidad(Cuenta cuenta) => new(
        cuenta.Id,
        cuenta.NumeroCuenta,
        cuenta.TipoCuenta.ToString(),
        cuenta.SaldoInicial,
        cuenta.Saldo,
        cuenta.Estado,
        cuenta.ClienteIdentificacion);
}
