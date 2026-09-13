# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CuentaService.Api/CuentaService.Api.csproj", "CuentaService.Api/"]
COPY ["CuentaService.Aplicacion/CuentaService.Aplicacion.csproj", "CuentaService.Aplicacion/"]
COPY ["CuentaService.Dominio/CuentaService.Dominio.csproj", "CuentaService.Dominio/"]
COPY ["CuentaService.Infraestructura/CuentaService.Infraestructura.csproj", "CuentaService.Infraestructura/"]
COPY ["Shared.Contracts/Shared.Contracts.csproj", "Shared.Contracts/"]
RUN dotnet restore "./CuentaService.Api/CuentaService.Api.csproj"
COPY . .
WORKDIR "/src/CuentaService.Api"
RUN dotnet build "./CuentaService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CuentaService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CuentaService.Api.dll"]
