namespace CuentaService.Dominio;

public class Movimiento
{
    public int Id { get; private set; }
    public int CuentaId { get; private set; }
    public DateTime Fecha { get; private set; }
    public TipoMovimiento TipoMovimiento { get; private set; }

    // Positivo si es deposito, negativo si es retiro.
    public decimal Valor { get; private set; }

    // Saldo de la cuenta justo despues de este movimiento.
    public decimal Saldo { get; private set; }

    public virtual Cuenta Cuenta { get; private set; } = null!;

    protected Movimiento()
    {
    }

    // internal a proposito: solo Cuenta.RegistrarMovimiento puede crear uno de estos.
    internal Movimiento(Cuenta cuenta, TipoMovimiento tipoMovimiento, decimal valorConSigno, decimal saldoResultante, DateTime fecha)
    {
        Cuenta = cuenta;
        TipoMovimiento = tipoMovimiento;
        Valor = valorConSigno;
        Saldo = saldoResultante;
        Fecha = fecha;
    }
}
