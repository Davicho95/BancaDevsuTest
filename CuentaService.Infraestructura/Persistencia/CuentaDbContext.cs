using CuentaService.Dominio;
using Microsoft.EntityFrameworkCore;

namespace CuentaService.Infraestructura.Persistencia;

public class CuentaDbContext : DbContext
{
    public CuentaDbContext(DbContextOptions<CuentaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cuenta> Cuentas => Set<Cuenta>();

    public DbSet<Movimiento> Movimientos => Set<Movimiento>();

    public DbSet<ClienteInfo> ClientesInfo => Set<ClienteInfo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cuenta>(entity =>
        {
            entity.ToTable("cuentas");
            entity.HasKey(e => e.Id).HasName("cuentas_pkey");

            entity.Property(e => e.Id).HasColumnName("cuenta_id");
            entity.Property(e => e.NumeroCuenta).HasColumnName("numero_cuenta").HasMaxLength(20).IsRequired();
            entity.HasIndex(e => e.NumeroCuenta).IsUnique();

            entity.Property(e => e.TipoCuenta)
                .HasColumnName("tipo_cuenta")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.SaldoInicial).HasColumnName("saldo_inicial").HasPrecision(18, 2);
            entity.Property(e => e.Saldo).HasColumnName("saldo").HasPrecision(18, 2);
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.ClienteIdentificacion).HasColumnName("cliente_identificacion").HasMaxLength(20).IsRequired();

            entity.HasMany(e => e.Movimientos)
                .WithOne(m => m.Cuenta)
                .HasForeignKey(m => m.CuentaId)
                .HasConstraintName("movimientos_cuenta_id_fkey");
        });

        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.ToTable("movimientos");
            entity.HasKey(e => e.Id).HasName("movimientos_pkey");

            entity.Property(e => e.Id).HasColumnName("movimiento_id");
            entity.Property(e => e.CuentaId).HasColumnName("cuenta_id");
            entity.Property(e => e.Fecha).HasColumnName("fecha").HasColumnType("timestamp with time zone");

            entity.Property(e => e.TipoMovimiento)
                .HasColumnName("tipo_movimiento")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.Valor).HasColumnName("valor").HasPrecision(18, 2);
            entity.Property(e => e.Saldo).HasColumnName("saldo").HasPrecision(18, 2);

            entity.HasIndex(e => new { e.CuentaId, e.Fecha });
        });

        modelBuilder.Entity<ClienteInfo>(entity =>
        {
            entity.ToTable("clientes_info");
            entity.HasKey(e => e.ClienteIdentificacion);

            entity.Property(e => e.ClienteIdentificacion).HasColumnName("cliente_identificacion").HasMaxLength(20);
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Estado).HasColumnName("estado");
        });
    }
}
