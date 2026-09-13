using CuentaService.Dominio;
using CuentaService.Dominio.Repositorios;
using CuentaService.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace CuentaService.Infraestructura.Repositorios;

public class MovimientoRepository : IMovimientoRepository
{
    private readonly CuentaDbContext _context;

    public MovimientoRepository(CuentaDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Movimiento>> ObtenerPorClienteYRangoAsync(
        string clienteIdentificacion,
        DateTime desde,
        DateTime hasta,
        CancellationToken cancellationToken = default) =>
        await _context.Movimientos
            .Include(m => m.Cuenta)
            .Where(m => m.Cuenta.ClienteIdentificacion == clienteIdentificacion
                        && m.Fecha >= desde
                        && m.Fecha <= hasta)
            .OrderBy(m => m.Fecha)
            .ToListAsync(cancellationToken);
}
