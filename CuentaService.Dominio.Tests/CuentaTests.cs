using CuentaService.Dominio.Excepciones;

namespace CuentaService.Dominio.Tests;

public class CuentaTests
{
    private static Cuenta CrearCuentaValida(decimal saldoInicial = 1000m) =>
        new("478758", TipoCuenta.Ahorros, saldoInicial, "1712345678");

    [Fact]
    public void Constructor_ConDatosValidos_InicializaSaldoIgualASaldoInicial()
    {
        var cuenta = CrearCuentaValida(500m);

        Assert.Equal(500m, cuenta.SaldoInicial);
        Assert.Equal(500m, cuenta.Saldo);
        Assert.True(cuenta.Estado);
        Assert.Empty(cuenta.Movimientos);
    }

    [Fact]
    public void Constructor_ConSaldoInicialNegativo_LanzaCuentaInvalidaException()
    {
        Assert.Throws<CuentaInvalidaException>(() => new Cuenta("478758", TipoCuenta.Ahorros, -1m, "1712345678"));
    }

    [Fact]
    public void RegistrarMovimiento_Deposito_IncrementaSaldoYRegistraMovimientoPositivo()
    {
        var cuenta = CrearCuentaValida(1000m);

        var movimiento = cuenta.RegistrarMovimiento(TipoMovimiento.Deposito, 200m);

        Assert.Equal(1200m, cuenta.Saldo);
        Assert.Equal(200m, movimiento.Valor);
        Assert.Equal(1200m, movimiento.Saldo);
        Assert.Single(cuenta.Movimientos);
    }

    [Fact]
    public void RegistrarMovimiento_RetiroConSaldoSuficiente_DecrementaSaldoYRegistraMovimientoNegativo()
    {
        var cuenta = CrearCuentaValida(1000m);

        var movimiento = cuenta.RegistrarMovimiento(TipoMovimiento.Retiro, 300m);

        Assert.Equal(700m, cuenta.Saldo);
        Assert.Equal(-300m, movimiento.Valor);
    }

    [Fact]
    public void RegistrarMovimiento_RetiroMayorAlSaldo_LanzaSaldoNoDisponibleException()
    {
        var cuenta = CrearCuentaValida(100m);

        var excepcion = Assert.Throws<SaldoNoDisponibleException>(() => cuenta.RegistrarMovimiento(TipoMovimiento.Retiro, 500m));

        Assert.Equal("Saldo no disponible", excepcion.Message);
        Assert.Equal(100m, cuenta.Saldo); // el saldo no debe cambiar si la operacion falla
    }

    [Fact]
    public void RegistrarMovimiento_ConValorCeroONegativo_LanzaMovimientoInvalidoException()
    {
        var cuenta = CrearCuentaValida(100m);

        Assert.Throws<MovimientoInvalidoException>(() => cuenta.RegistrarMovimiento(TipoMovimiento.Deposito, 0m));
    }

    [Fact]
    public void RegistrarMovimiento_SobreCuentaInactiva_LanzaCuentaInvalidaException()
    {
        var cuenta = CrearCuentaValida(100m);
        cuenta.Desactivar();

        Assert.Throws<CuentaInvalidaException>(() => cuenta.RegistrarMovimiento(TipoMovimiento.Deposito, 50m));
    }
}
