namespace CuentaService.Dominio.Excepciones;

// El texto del mensaje es un requisito de negocio explicito, no tocarlo.
public class SaldoNoDisponibleException : DomainException
{
    public SaldoNoDisponibleException() : base("Saldo no disponible")
    {
    }
}
