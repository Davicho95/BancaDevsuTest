using System.Text.Json.Serialization;

namespace CuentaService.Aplicacion.Dtos;

// Los nombres de propiedad con espacios ("Numero Cuenta", etc.) son un requisito del negocio, no un typo.
public record MovimientoReporteDto(
    [property: JsonPropertyName("Fecha")] DateTime Fecha,
    [property: JsonPropertyName("Cliente")] string Cliente,
    [property: JsonPropertyName("Numero Cuenta")] string NumeroCuenta,
    [property: JsonPropertyName("Tipo")] string Tipo,
    [property: JsonPropertyName("Saldo Inicial")] decimal SaldoInicial,
    [property: JsonPropertyName("Estado")] bool Estado,
    [property: JsonPropertyName("Movimiento")] decimal Movimiento,
    [property: JsonPropertyName("Saldo Disponible")] decimal SaldoDisponible);
