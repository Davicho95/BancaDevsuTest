namespace CuentaService.Dominio.Excepciones;

public class CuentaInvalidaException : DomainException
{
    public CuentaInvalidaException(string message) : base(message)
    {
    }
}
