namespace ClienteService.Dominio.Excepciones;

// Base comun para poder capturar cualquier violacion de regla de negocio desde un solo catch en la Api.
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }
}
