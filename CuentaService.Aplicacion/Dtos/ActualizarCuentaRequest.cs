namespace CuentaService.Aplicacion.Dtos;

// Solo el estado se puede editar: numero, tipo y saldo no cambian despues de abrir la cuenta
// (el saldo se mueve unicamente via RegistrarMovimiento).
public record ActualizarCuentaRequest(bool Estado);
