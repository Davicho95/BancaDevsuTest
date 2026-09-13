using CuentaService.Aplicacion;
using CuentaService.Api.Middleware;
using CuentaService.Infraestructura;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CuentaService API",
        Version = "v1",
        Description = "Gestion de Cuenta, Movimiento y reportes."
    });
});

builder.Services.AddInfraestructura(builder.Configuration);
builder.Services.AddAplicacion();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CuentaService API v1");
    c.RoutePrefix = "swagger";
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// top-level statements dejan Program como internal; esto lo hace visible para el proyecto de tests.
public partial class Program;
