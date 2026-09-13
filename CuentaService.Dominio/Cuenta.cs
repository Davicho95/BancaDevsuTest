using CuentaService.Dominio.Excepciones;

namespace CuentaService.Dominio;

// Aggregate root: los movimientos siempre se crean aca dentro, nunca instanciando Movimiento por fuera.
public class Cuenta
{
    // El nombre del campo importa: EF Core lo detecta por convencion como backing field de Movimientos.
    private readonly List<Movimiento> _movimientos = new();

    public int Id { get; private set; }
    public string NumeroCuenta { get; private set; } = string.Empty;
    public TipoCuenta TipoCuenta { get; private set; }
    public decimal SaldoInicial { get; private set; }
    public decimal Saldo { get; private set; }
    public bool Estado { get; private set; }

    // Sin FK real: Cliente vive en otra base/otro servicio. La relacion se resuelve por este valor.
    public string ClienteIdentificacion { get; private set; } = string.Empty;

    public IReadOnlyCollection<Movimiento> Movimientos => _movimientos.AsReadOnly();

    protected Cuenta()
    {
    }

    public Cuenta(string numeroCuenta, TipoCuenta tipoCuenta, decimal saldoInicial, string clienteIdentificacion)
    {
        if (string.IsNullOrWhiteSpace(numeroCuenta))
            throw new CuentaInvalidaException("El numero de cuenta es obligatorio.");
        if (saldoInicial < 0)
            throw new CuentaInvalidaException("El saldo inicial no puede ser negativo.");
        if (string.IsNullOrWhiteSpace(clienteIdentificacion))
            throw new CuentaInvalidaException("La identificacion del cliente es obligatoria.");

        NumeroCuenta = numeroCuenta.Trim();
        TipoCuenta = tipoCuenta;
        SaldoInicial = saldoInicial;
        Saldo = saldoInicial;
        ClienteIdentificacion = clienteIdentificacion.Trim();
        Estado = true;
    }

    public Movimiento RegistrarMovimiento(TipoMovimiento tipo, decimal valor, DateTime? fecha = null)
    {
        if (!Estado)
            throw new CuentaInvalidaException("No se pueden registrar movimientos en una cuenta inactiva.");
        if (valor <= 0)
            throw new MovimientoInvalidoException("El valor del movimiento debe ser mayor a cero.");

        var valorConSigno = tipo == TipoMovimiento.Retiro ? -valor : valor;
        var nuevoSaldo = Saldo + valorConSigno;

        if (nuevoSaldo < 0)
            throw new SaldoNoDisponibleException();

        Saldo = nuevoSaldo;

        var movimiento = new Movimiento(this, tipo, valorConSigno, Saldo, fecha ?? DateTime.UtcNow);
        _movimientos.Add(movimiento);
        return movimiento;
    }

    public void Activar() => Estado = true;

    public void Desactivar() => Estado = false;
}
