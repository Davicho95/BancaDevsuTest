namespace ClienteService.Dominio.Repositorios;

public interface IClienteRepository
{
    Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Cliente?> ObtenerPorClienteIdAsync(string clienteId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task AgregarAsync(Cliente cliente, CancellationToken cancellationToken = default);

    Task ActualizarAsync(Cliente cliente, CancellationToken cancellationToken = default);

    Task EliminarAsync(Cliente cliente, CancellationToken cancellationToken = default);
}
