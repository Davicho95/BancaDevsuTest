namespace Shared.Contracts;

// Publicado por ClienteService cuando un Cliente se crea, se actualiza o cambia de estado.
// CuentaService lo consume para no tener que llamarnos por HTTP cada vez que arma el reporte.
public record ClienteActualizadoEvent(string ClienteIdentificacion, string Nombre, bool Estado);
