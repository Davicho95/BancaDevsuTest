using ClienteService.Dominio;
using ClienteService.Dominio.Repositorios;
using ClienteService.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace ClienteService.Infraestructura.Repositorios;

public class ClienteRepository : IClienteRepository
{
    private readonly ClienteDbContext _context;

    public ClienteRepository(ClienteDbContext context)
    {
        _context = context;
    }

    public Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default) =>
        _context.Clientes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<Cliente?> ObtenerPorClienteIdAsync(string clienteId, CancellationToken cancellationToken = default) =>
        _context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == clienteId, cancellationToken);

    public async Task<IReadOnlyList<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default) =>
        await _context.Clientes.OrderBy(c => c.Id).ToListAsync(cancellationToken);

    public async Task AgregarAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        await _context.Clientes.AddAsync(cliente, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task EliminarAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
