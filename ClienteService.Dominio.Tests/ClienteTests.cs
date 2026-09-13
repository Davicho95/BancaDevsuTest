using ClienteService.Dominio.Excepciones;

namespace ClienteService.Dominio.Tests;

public class ClienteTests
{
    private static Cliente CrearClienteValido() => new(
        clienteId: "jperez",
        contrasena: "hash-seguro",
        nombre: "Juan Perez",
        genero: "Masculino",
        edad: 30,
        identificacion: "1712345678",
        direccion: "Av. Siempre Viva 123",
        telefono: "0991234567");

    [Fact]
    public void Constructor_ConDatosValidos_CreaClienteActivo()
    {
        var cliente = CrearClienteValido();

        Assert.Equal("jperez", cliente.ClienteId);
        Assert.Equal("Juan Perez", cliente.Nombre);
        Assert.Equal("1712345678", cliente.Identificacion);
        Assert.True(cliente.Estado);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ConClienteIdInvalido_LanzaClienteInvalidoException(string clienteIdInvalido)
    {
        Assert.Throws<ClienteInvalidoException>(() => new Cliente(
            clienteIdInvalido, "hash", "Juan Perez", "Masculino", 30, "1712345678", "Direccion", "0991234567"));
    }

    [Fact]
    public void Constructor_ConContrasenaVacia_LanzaClienteInvalidoException()
    {
        Assert.Throws<ClienteInvalidoException>(() => new Cliente(
            "jperez", "", "Juan Perez", "Masculino", 30, "1712345678", "Direccion", "0991234567"));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(121)]
    public void Constructor_ConEdadFueraDeRango_LanzaPersonaInvalidaException(int edadInvalida)
    {
        Assert.Throws<PersonaInvalidaException>(() => new Cliente(
            "jperez", "hash", "Juan Perez", "Masculino", edadInvalida, "1712345678", "Direccion", "0991234567"));
    }

    [Fact]
    public void Constructor_ConNombreVacio_LanzaPersonaInvalidaException()
    {
        Assert.Throws<PersonaInvalidaException>(() => new Cliente(
            "jperez", "hash", "", "Masculino", 30, "1712345678", "Direccion", "0991234567"));
    }

    [Fact]
    public void ActualizarInformacion_ConDatosValidos_ActualizaCamposHeredadosDePersona()
    {
        var cliente = CrearClienteValido();

        cliente.ActualizarInformacion("Juan Carlos Perez", "Masculino", 31, "Nueva Direccion", "0987654321");

        Assert.Equal("Juan Carlos Perez", cliente.Nombre);
        Assert.Equal(31, cliente.Edad);
        Assert.Equal("Nueva Direccion", cliente.Direccion);
        Assert.Equal("0987654321", cliente.Telefono);
        Assert.Equal("1712345678", cliente.Identificacion); // no editable
    }

    [Fact]
    public void Desactivar_CambiaEstadoAFalse()
    {
        var cliente = CrearClienteValido();

        cliente.Desactivar();

        Assert.False(cliente.Estado);
    }

    [Fact]
    public void Activar_DespuesDeDesactivar_RestauraEstadoATrue()
    {
        var cliente = CrearClienteValido();
        cliente.Desactivar();

        cliente.Activar();

        Assert.True(cliente.Estado);
    }

    [Fact]
    public void CambiarContrasena_ConValorVacio_LanzaClienteInvalidoException()
    {
        var cliente = CrearClienteValido();

        Assert.Throws<ClienteInvalidoException>(() => cliente.CambiarContrasena(""));
    }

    [Fact]
    public void CambiarContrasena_ConValorValido_ActualizaContrasena()
    {
        var cliente = CrearClienteValido();

        cliente.CambiarContrasena("nuevo-hash");

        Assert.Equal("nuevo-hash", cliente.Contrasena);
    }
}
