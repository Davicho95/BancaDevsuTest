namespace CuentaService.Dominio.Excepciones;

public class MovimientoInvalidoException : DomainException
{
    public MovimientoInvalidoException(string message) : base(message)
    {
    }
}
