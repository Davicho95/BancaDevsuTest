namespace ClienteService.Aplicacion.Dtos;

public record CrearClienteRequest(
    string ClienteId,
    string Contrasena,
    string Nombre,
    string Genero,
    int Edad,
    string Identificacion,
    string Direccion,
    string Telefono);
