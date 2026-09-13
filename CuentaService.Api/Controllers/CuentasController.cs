using CuentaService.Aplicacion;
using CuentaService.Aplicacion.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CuentaService.Api.Controllers;

// Sin Delete a proposito: una cuenta se desactiva, no se borra.
[ApiController]
[Route("cuentas")]
public class CuentasController : ControllerBase
{
    private readonly ICuentaAppService _cuentaAppService;

    public CuentasController(ICuentaAppService cuentaAppService)
    {
        _cuentaAppService = cuentaAppService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CuentaDto>>>> ObtenerTodas(CancellationToken cancellationToken)
        => Ok((await _cuentaAppService.ObtenerTodasAsync(cancellationToken)).ToApiResponse());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CuentaDto>>> ObtenerPorId(int id, CancellationToken cancellationToken)
        => Ok((await _cuentaAppService.ObtenerPorIdAsync(id, cancellationToken)).ToApiResponse());

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CuentaDto>>> Crear(CrearCuentaRequest request, CancellationToken cancellationToken)
    {
        var cuenta = await _cuentaAppService.CrearAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = cuenta.Id }, cuenta.ToApiResponse());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<CuentaDto>>> Actualizar(int id, ActualizarCuentaRequest request, CancellationToken cancellationToken)
        => Ok((await _cuentaAppService.ActualizarAsync(id, request, cancellationToken)).ToApiResponse());
}
