using ClienteService.Aplicacion;
using ClienteService.Aplicacion.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ClienteService.Api.Controllers;

[ApiController]
[Route("clientes")]
public class ClientesController : ControllerBase
{
    private readonly IClienteAppService _clienteAppService;

    public ClientesController(IClienteAppService clienteAppService)
    {
        _clienteAppService = clienteAppService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ClienteDto>>>> ObtenerTodos(CancellationToken cancellationToken)
        => Ok((await _clienteAppService.ObtenerTodosAsync(cancellationToken)).ToApiResponse());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<ClienteDto>>> ObtenerPorId(int id, CancellationToken cancellationToken)
        => Ok((await _clienteAppService.ObtenerPorIdAsync(id, cancellationToken)).ToApiResponse());

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ClienteDto>>> Crear(CrearClienteRequest request, CancellationToken cancellationToken)
    {
        var cliente = await _clienteAppService.CrearAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.Id }, cliente.ToApiResponse());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<ClienteDto>>> Actualizar(int id, ActualizarClienteRequest request, CancellationToken cancellationToken)
        => Ok((await _clienteAppService.ActualizarAsync(id, request, cancellationToken)).ToApiResponse());

    // Solo toca el estado, el resto de los datos queda igual.
    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, CambiarEstadoRequest request, CancellationToken cancellationToken)
    {
        await _clienteAppService.CambiarEstadoAsync(id, request.Activo, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        await _clienteAppService.EliminarAsync(id, cancellationToken);
        return NoContent();
    }
}
