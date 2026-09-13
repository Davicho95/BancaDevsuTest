using ClienteService.Dominio.Excepciones;

namespace ClienteService.Dominio;

public class Cliente : Persona
{
    // Clave de login, distinta del Id tecnico que se hereda de Persona.
    public string ClienteId { get; private set; } = string.Empty;

    // Llega ya hasheada desde Aplicacion, al Dominio no le interesa el algoritmo.
    public string Contrasena { get; private set; } = string.Empty;

    public bool Estado { get; private set; }

    protected Cliente()
    {
    }

    public Cliente(
        string clienteId,
        string contrasena,
        string nombre,
        string genero,
        int edad,
        string identificacion,
        string direccion,
        string telefono)
        : base(nombre, genero, edad, identificacion, direccion, telefono)
    {
        if (string.IsNullOrWhiteSpace(clienteId))
            throw new ClienteInvalidoException("El clienteId es obligatorio.");
        if (string.IsNullOrWhiteSpace(contrasena))
            throw new ClienteInvalidoException("La contrasena es obligatoria.");

        ClienteId = clienteId.Trim();
        Contrasena = contrasena;
        Estado = true;
    }

    public void CambiarContrasena(string nuevaContrasena)
    {
        if (string.IsNullOrWhiteSpace(nuevaContrasena))
            throw new ClienteInvalidoException("La contrasena es obligatoria.");

        Contrasena = nuevaContrasena;
    }

    public void Activar() => Estado = true;

    public void Desactivar() => Estado = false;
}
