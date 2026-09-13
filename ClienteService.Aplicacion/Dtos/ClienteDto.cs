using ClienteService.Dominio;

namespace ClienteService.Aplicacion.Dtos;

public record ClienteDto(
    int Id,
    string ClienteId,
    string Nombre,
    string Genero,
    int Edad,
    string Identificacion,
    string Direccion,
    string Telefono,
    bool Estado)
{
    public static ClienteDto DesdeEntidad(Cliente cliente) => new(
        cliente.Id,
        cliente.ClienteId,
        cliente.Nombre,
        cliente.Genero,
        cliente.Edad,
        cliente.Identificacion,
        cliente.Direccion,
        cliente.Telefono,
        cliente.Estado);
}
