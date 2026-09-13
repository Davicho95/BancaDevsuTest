using System.Net;
using System.Net.Http.Json;
using CuentaService.Api;
using CuentaService.Aplicacion.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CuentaService.Api.IntegrationTests;

// Contra Postgres/RabbitMQ reales, sin mocks. Necesita "docker compose up -d postgres rabbitmq" corriendo.
public class CuentaFlujoIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CuentaFlujoIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Development")).CreateClient();
    }

    private static string NumeroCuentaUnico() => $"TEST-{Guid.NewGuid():N}"[..15];

    [Fact]
    public async Task CrearCuenta_RegistrarDeposito_ActualizaSaldoDisponible()
    {
        var numeroCuenta = NumeroCuentaUnico();
        var crearRequest = new CrearCuentaRequest(numeroCuenta, "Ahorros", 100m, "1712345678");

        var crearResponse = await _client.PostAsJsonAsync("/cuentas", crearRequest);
        crearResponse.EnsureSuccessStatusCode();
        var cuentaCreada = (await crearResponse.Content.ReadFromJsonAsync<ApiResponse<CuentaDto>>())?.Data;
        Assert.NotNull(cuentaCreada);
        Assert.Equal(100m, cuentaCreada!.Saldo);

        var movimientoRequest = new RegistrarMovimientoRequest(numeroCuenta, "Deposito", 50m);
        var movimientoResponse = await _client.PostAsJsonAsync("/movimientos", movimientoRequest);
        movimientoResponse.EnsureSuccessStatusCode();

        var cuentaActualizada = (await _client.GetFromJsonAsync<ApiResponse<CuentaDto>>($"/cuentas/{cuentaCreada.Id}"))?.Data;

        Assert.NotNull(cuentaActualizada);
        Assert.Equal(150m, cuentaActualizada!.Saldo);
    }

    [Fact]
    public async Task RegistrarRetiro_MayorAlSaldo_Retorna400ConMensajeSaldoNoDisponible()
    {
        var numeroCuenta = NumeroCuentaUnico();
        var crearRequest = new CrearCuentaRequest(numeroCuenta, "Ahorros", 10m, "1712345678");
        (await _client.PostAsJsonAsync("/cuentas", crearRequest)).EnsureSuccessStatusCode();

        var movimientoRequest = new RegistrarMovimientoRequest(numeroCuenta, "Retiro", 999m);
        var response = await _client.PostAsJsonAsync("/movimientos", movimientoRequest);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("Saldo no disponible", body);
    }
}
