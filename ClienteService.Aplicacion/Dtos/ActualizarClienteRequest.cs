namespace ClienteService.Aplicacion.Dtos;

// Identificacion y ClienteId no van aca a proposito: no se pueden cambiar despues del alta.
public record ActualizarClienteRequest(string Nombre, string Genero, int Edad, string Direccion, string Telefono);
