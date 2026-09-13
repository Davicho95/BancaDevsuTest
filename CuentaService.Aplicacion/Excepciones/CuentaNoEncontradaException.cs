using CuentaService.Dominio.Excepciones;

namespace CuentaService.Aplicacion.Excepciones;

public class CuentaNoEncontradaException : DomainException, INotFoundException
{
    public CuentaNoEncontradaException(int id) : base($"No se encontro la cuenta con id {id}.")
    {
    }

    public CuentaNoEncontradaException(string numeroCuenta) : base($"No se encontro la cuenta '{numeroCuenta}'.")
    {
    }
}
