namespace ClienteService.Dominio.Excepciones;

public class ClienteInvalidoException : DomainException
{
    public ClienteInvalidoException(string message) : base(message)
    {
    }
}
