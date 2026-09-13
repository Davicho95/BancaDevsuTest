namespace ClienteService.Dominio.Excepciones;

public class PersonaInvalidaException : DomainException
{
    public PersonaInvalidaException(string message) : base(message)
    {
    }
}
