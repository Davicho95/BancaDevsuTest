using CuentaService.Dominio;
using CuentaService.Dominio.Repositorios;
using CuentaService.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace CuentaService.Infraestructura.Repositorios;

public class CuentaRepository : ICuentaRepository
{
    private readonly CuentaDbContext _context;

    public CuentaRepository(CuentaDbContext context)
    {
        _context = context;
    }

    public Task<Cuenta?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default) =>
        _context.Cuentas.Include(c => c.Movimientos).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<Cuenta?> ObtenerPorNumeroCuentaAsync(string numeroCuenta, CancellationToken cancellationToken = default) =>
        _context.Cuentas.Include(c => c.Movimientos).FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta, cancellationToken);

    public async Task<IReadOnlyList<Cuenta>> ObtenerPorClienteAsync(string clienteIdentificacion, CancellationToken cancellationToken = default) =>
        await _context.Cuentas
            .Where(c => c.ClienteIdentificacion == clienteIdentificacion)
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Cuenta>> ObtenerTodasAsync(CancellationToken cancellationToken = default) =>
        await _context.Cuentas.OrderBy(c => c.Id).ToListAsync(cancellationToken);

    public async Task AgregarAsync(Cuenta cuenta, CancellationToken cancellationToken = default)
    {
        await _context.Cuentas.AddAsync(cuenta, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarAsync(Cuenta cuenta, CancellationToken cancellationToken = default)
    {
        _context.Cuentas.Update(cuenta);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
