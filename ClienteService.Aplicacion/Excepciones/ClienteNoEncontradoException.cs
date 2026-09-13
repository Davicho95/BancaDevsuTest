using ClienteService.Dominio.Excepciones;

namespace ClienteService.Aplicacion.Excepciones;

public class ClienteNoEncontradoException : DomainException, INotFoundException
{
    public ClienteNoEncontradoException(int id) : base($"No se encontro el cliente con id {id}.")
    {
    }

    public ClienteNoEncontradoException(string clienteId) : base($"No se encontro el cliente '{clienteId}'.")
    {
    }
}
