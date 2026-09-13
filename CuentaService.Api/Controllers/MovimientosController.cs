using CuentaService.Aplicacion;
using CuentaService.Aplicacion.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CuentaService.Api.Controllers;

// Solo Crear + Leer, sin Update (ver IMovimientoAppService).
[ApiController]
[Route("movimientos")]
public class MovimientosController : ControllerBase
{
    private readonly IMovimientoAppService _movimientoAppService;

    public MovimientosController(IMovimientoAppService movimientoAppService)
    {
        _movimientoAppService = movimientoAppService;
    }

    [HttpGet("cuenta/{cuentaId:int}")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<MovimientoDto>>>> ObtenerPorCuenta(int cuentaId, CancellationToken cancellationToken)
        => Ok((await _movimientoAppService.ObtenerPorCuentaAsync(cuentaId, cancellationToken)).ToApiResponse());

    [HttpPost]
    public async Task<ActionResult<ApiResponse<MovimientoDto>>> Registrar(RegistrarMovimientoRequest request, CancellationToken cancellationToken)
    {
        var movimiento = await _movimientoAppService.RegistrarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorCuenta), new { cuentaId = movimiento.CuentaId }, movimiento.ToApiResponse());
    }
}
