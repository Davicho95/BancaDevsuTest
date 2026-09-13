using CuentaService.Dominio.Repositorios;
using CuentaService.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace CuentaService.Infraestructura.Repositorios;

public class ClienteInfoRepository : IClienteInfoRepository
{
    private readonly CuentaDbContext _context;

    public ClienteInfoRepository(CuentaDbContext context)
    {
        _context = context;
    }

    public async Task GuardarOActualizarAsync(string clienteIdentificacion, string nombre, bool estado, CancellationToken cancellationToken = default)
    {
        var existente = await _context.ClientesInfo.FindAsync([clienteIdentificacion], cancellationToken);

        if (existente is null)
        {
            _context.ClientesInfo.Add(new ClienteInfo
            {
                ClienteIdentificacion = clienteIdentificacion,
                Nombre = nombre,
                Estado = estado
            });
        }
        else
        {
            existente.Nombre = nombre;
            existente.Estado = estado;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<string?> ObtenerNombreAsync(string clienteIdentificacion, CancellationToken cancellationToken = default)
    {
        var info = await _context.ClientesInfo.FirstOrDefaultAsync(c => c.ClienteIdentificacion == clienteIdentificacion, cancellationToken);
        return info?.Nombre;
    }
}
