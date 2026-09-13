namespace CuentaService.Dominio.Repositorios;

// Cache local de nombre/estado del cliente, poblada por el consumer de RabbitMQ.
// Existe para no tener que llamar por HTTP a ClienteService cada vez que se arma el reporte.
public interface IClienteInfoRepository
{
    Task GuardarOActualizarAsync(string clienteIdentificacion, string nombre, bool estado, CancellationToken cancellationToken = default);

    Task<string?> ObtenerNombreAsync(string clienteIdentificacion, CancellationToken cancellationToken = default);
}
