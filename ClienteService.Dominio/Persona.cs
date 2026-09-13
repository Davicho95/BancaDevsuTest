using ClienteService.Dominio.Excepciones;

namespace ClienteService.Dominio;

// No tiene endpoint propio, se usa siempre a traves de Cliente.
public abstract class Persona
{
    public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Genero { get; private set; } = string.Empty;
    public int Edad { get; private set; }
    public string Identificacion { get; private set; } = string.Empty;
    public string Direccion { get; private set; } = string.Empty;
    public string Telefono { get; private set; } = string.Empty;

    // EF Core necesita esto para materializar la entidad.
    protected Persona()
    {
    }

    protected Persona(string nombre, string genero, int edad, string identificacion, string direccion, string telefono)
    {
        ValidarDatosBasicos(nombre, genero, edad, direccion, telefono);
        if (string.IsNullOrWhiteSpace(identificacion))
            throw new PersonaInvalidaException("La identificacion es obligatoria.");

        Nombre = nombre.Trim();
        Genero = genero.Trim();
        Edad = edad;
        Identificacion = identificacion.Trim();
        Direccion = direccion.Trim();
        Telefono = telefono.Trim();
    }

    // La identificacion queda fuera: no se puede tocar despues del alta.
    public void ActualizarInformacion(string nombre, string genero, int edad, string direccion, string telefono)
    {
        ValidarDatosBasicos(nombre, genero, edad, direccion, telefono);

        Nombre = nombre.Trim();
        Genero = genero.Trim();
        Edad = edad;
        Direccion = direccion.Trim();
        Telefono = telefono.Trim();
    }

    private static void ValidarDatosBasicos(string nombre, string genero, int edad, string direccion, string telefono)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new PersonaInvalidaException("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(genero))
            throw new PersonaInvalidaException("El genero es obligatorio.");
        if (edad < 0 || edad > 120)
            throw new PersonaInvalidaException("La edad debe estar entre 0 y 120.");
        if (string.IsNullOrWhiteSpace(direccion))
            throw new PersonaInvalidaException("La direccion es obligatoria.");
        if (string.IsNullOrWhiteSpace(telefono))
            throw new PersonaInvalidaException("El telefono es obligatorio.");
    }
}
