namespace CuentaService.Aplicacion.Dtos;

// TipoCuenta llega como texto ("Ahorros"/"Corriente") y se parsea/valida en el caso de uso.
public record CrearCuentaRequest(string NumeroCuenta, string TipoCuenta, decimal SaldoInicial, string ClienteIdentificacion);
