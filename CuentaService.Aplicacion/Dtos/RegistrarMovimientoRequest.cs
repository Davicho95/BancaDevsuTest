namespace CuentaService.Aplicacion.Dtos;

// TipoMovimiento como texto ("Deposito"/"Retiro"); Valor siempre positivo, el signo lo pone el Dominio.
public record RegistrarMovimientoRequest(string NumeroCuenta, string TipoMovimiento, decimal Valor);
