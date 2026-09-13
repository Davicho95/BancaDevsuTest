# BancaDevsuTest

Prueba técnica de arquitectura de microservicios para banca. Dos servicios en .NET 10 / ASP.NET Core, cada uno con su propia base PostgreSQL y su propia solución en capas (Clean Architecture + Repository), comunicados de forma asíncrona vía RabbitMQ.

## Los dos servicios

**ClienteService** — Persona y Cliente (`Cliente` hereda de `Persona`, mapeado como una sola tabla vía TPH en EF Core).

**CuentaService** — Cuenta y Movimiento. `Cuenta` es el aggregate root; un `Movimiento` solo puede nacer de `Cuenta.RegistrarMovimiento(...)`, nunca suelto.

No hay ninguna FK entre las dos bases porque son bounded contexts distintos. Cuando se crea/actualiza/desactiva un Cliente, ClienteService publica `ClienteActualizadoEvent` por RabbitMQ (MassTransit), y CuentaService lo consume para mantener una copia local de solo lectura del nombre — así el reporte de estado de cuenta puede mostrar el nombre del cliente sin tener que llamar por HTTP al otro servicio cada vez.

Cada capa de Dominio (`ClienteService.Dominio`, `CuentaService.Dominio`) no tiene ningún paquete NuGet — ni EF Core, ni nada. Solo la capa de Infraestructura conoce Npgsql/EF Core/MassTransit; el Dominio es C# puro.

## Estructura

```
ClienteService.Dominio / .Aplicacion / .Infraestructura / .Api
CuentaService.Dominio  / .Aplicacion / .Infraestructura / .Api
Shared.Contracts                        contrato del evento de integracion
ClienteService.Dominio.Tests            unit tests de Cliente
CuentaService.Dominio.Tests             unit tests de Cuenta
CuentaService.Api.IntegrationTests      pruebas de integracion end-to-end
BaseDatos.sql                           esquema + datos semilla
docker-compose.yml
BancaDevsuTest.postman_collection.json
```

## Levantar todo

```bash
docker compose up --build -d
```

| Servicio | URL |
|---|---|
| ClienteService | http://localhost:5001 (Swagger en `/swagger`) |
| CuentaService | http://localhost:5002 (Swagger en `/swagger`) |
| Postgres | localhost:5433 |
| RabbitMQ (panel) | http://localhost:15672 (`banca_user` / `banca_pass`) |

`docker compose ps` para ver que todo esté sano. `docker compose down` baja el stack sin perder datos; `docker compose down -v` lo resetea del todo (vuelve a correr `BaseDatos.sql` desde cero).

## Endpoints

**ClienteService**

- `GET /clientes`, `GET /clientes/{id}`
- `POST /clientes`
- `PUT /clientes/{id}` — actualiza datos personales
- `PATCH /clientes/{id}/estado` — activar/desactivar
- `DELETE /clientes/{id}`

**CuentaService**

- `GET /cuentas`, `GET /cuentas/{id}`
- `POST /cuentas`
- `PUT /cuentas/{id}` — activar/desactivar (numero, tipo y saldo no se editan)
- `GET /movimientos/cuenta/{cuentaId}`
- `POST /movimientos` — deposito o retiro; si no hay saldo devuelve 400 con `"Saldo no disponible"`
- `GET /reportes?cliente={identificacion}&fechaInicio={fecha}&fechaFin={fecha}` — estado de cuenta

`cliente` en `/reportes` es la identificación (cédula), la misma clave que usa el evento entre los dos servicios — no el usuario de login.

Las respuestas exitosas (200/201) van envueltas en `{ "data": ..., "timestamp": ... }`; los errores se quedan tal cual en formato `ProblemDetails` (`{ "title", "status", "detail" }`), sin envoltura extra.

La colección de Postman trae todo esto con ejemplos, incluyendo el caso de saldo insuficiente.

## Datos semilla

`BaseDatos.sql` crea a Jose Lema, Marianela Montalvo y Juan Osorio con sus cuentas, ya con los movimientos de ejemplo aplicados sobre el saldo.

## Correr sin Docker

```bash
docker compose up -d postgres rabbitmq
dotnet run --project ClienteService.Api
dotnet run --project CuentaService.Api
```

Cada API detecta que corre fuera de contenedor (`appsettings.Development.json`) y apunta a `localhost` en vez de a los nombres de servicio de Docker.

## Tests

```bash
dotnet test ClienteService.Dominio.Tests
dotnet test CuentaService.Dominio.Tests
```

Las de integración necesitan Postgres y RabbitMQ arriba:

```bash
docker compose up -d postgres rabbitmq
dotnet test CuentaService.Api.IntegrationTests
```

## Algunas decisiones que vale la pena explicar

- **Cliente hereda de Persona de verdad**, no es solo una relación por FK — se mapea con Table-Per-Hierarchy en EF Core para que la tabla siga siendo una sola.
- **Contraseña hasheada con PBKDF2** (usando solo lo que trae el BCL, sin meter una librería nueva) — no había forma de justificar guardarla en texto plano aunque el ejercicio no pida login.
- **Cuenta no tiene Delete y Movimiento no tiene Update.** Una cuenta se desactiva en vez de borrarse (por su historial); un movimiento ya registrado no se debería poder tocar. No son huecos, son decisiones.
- **RabbitMQ en vez de una llamada HTTP directa** entre los dos servicios para resolver el nombre del cliente en el reporte — justo el punto de tener microservicios separados es no acoplarlos así.
- **Cosas que quedaron pensadas pero no implementadas** por alcance: outbox pattern para no perder el evento si RabbitMQ está caído justo cuando se publica, reintentos con backoff en el consumer, y escalar cada Api horizontalmente (son stateless, no debería haber problema).
