using ClienteService.Dominio;
using Microsoft.EntityFrameworkCore;

namespace ClienteService.Infraestructura.Persistencia;

// Persona no tiene su propio ToTable, entonces EF la mapea junto con Cliente en la misma tabla (TPH).
public class ClienteDbContext : DbContext
{
    public ClienteDbContext(DbContextOptions<ClienteDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("clientes");
            entity.HasKey(c => c.Id).HasName("clientes_pkey");

            entity.Property(c => c.Id).HasColumnName("id");

            entity.Property(c => c.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            entity.Property(c => c.Genero).HasColumnName("genero").HasMaxLength(20).IsRequired();
            entity.Property(c => c.Edad).HasColumnName("edad").IsRequired();
            entity.Property(c => c.Identificacion).HasColumnName("identificacion").HasMaxLength(20).IsRequired();
            entity.HasIndex(c => c.Identificacion).IsUnique();
            entity.Property(c => c.Direccion).HasColumnName("direccion").HasMaxLength(250).IsRequired();
            entity.Property(c => c.Telefono).HasColumnName("telefono").HasMaxLength(20).IsRequired();

            entity.Property(c => c.ClienteId).HasColumnName("cliente_id").HasMaxLength(20).IsRequired();
            entity.HasIndex(c => c.ClienteId).IsUnique();
            entity.Property(c => c.Contrasena).HasColumnName("contrasena").HasMaxLength(250).IsRequired();
            entity.Property(c => c.Estado).HasColumnName("estado").IsRequired();
        });
    }
}
