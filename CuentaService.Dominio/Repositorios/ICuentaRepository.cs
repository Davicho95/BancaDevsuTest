namespace CuentaService.Dominio.Repositorios;

public interface ICuentaRepository
{
    Task<Cuenta?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Cuenta?> ObtenerPorNumeroCuentaAsync(string numeroCuenta, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Cuenta>> ObtenerPorClienteAsync(string clienteIdentificacion, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Cuenta>> ObtenerTodasAsync(CancellationToken cancellationToken = default);

    Task AgregarAsync(Cuenta cuenta, CancellationToken cancellationToken = default);

    Task ActualizarAsync(Cuenta cuenta, CancellationToken cancellationToken = default);
}
