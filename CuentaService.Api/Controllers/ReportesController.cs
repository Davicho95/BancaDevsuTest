using CuentaService.Aplicacion;
using CuentaService.Aplicacion.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CuentaService.Api.Controllers;

// "cliente" es la identificacion (cedula), no el clienteId de login -- es la misma clave que
// usa ClienteActualizadoEvent para correlacionar los dos servicios.
[ApiController]
[Route("reportes")]
public class ReportesController : ControllerBase
{
    private readonly IReporteAppService _reporteAppService;

    public ReportesController(IReporteAppService reporteAppService)
    {
        _reporteAppService = reporteAppService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<MovimientoReporteDto>>>> Obtener(
        [FromQuery] string cliente,
        [FromQuery] DateTime fechaInicio,
        [FromQuery] DateTime fechaFin,
        CancellationToken cancellationToken)
        => Ok((await _reporteAppService.GenerarEstadoCuentaAsync(cliente, fechaInicio, fechaFin, cancellationToken)).ToApiResponse());
}
