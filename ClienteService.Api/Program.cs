using ClienteService.Aplicacion;
using ClienteService.Api.Middleware;
using ClienteService.Infraestructura;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ClienteService API",
        Version = "v1",
        Description = "Gestion de Persona y Cliente."
    });
});

builder.Services.AddInfraestructura(builder.Configuration);
builder.Services.AddAplicacion();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClienteService API v1");
    c.RoutePrefix = "swagger";
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
