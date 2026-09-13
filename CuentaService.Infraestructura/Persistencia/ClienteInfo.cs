namespace CuentaService.Infraestructura.Persistencia;

// No es una entidad de Dominio, es solo la cache que llena el consumer de RabbitMQ.
public class ClienteInfo
{
    public string ClienteIdentificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public bool Estado { get; set; }
}
