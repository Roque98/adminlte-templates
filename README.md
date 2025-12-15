# AdminLTE Template con ASP.NET Core y Dapper

Template de proyecto ASP.NET Core 6.0 con AdminLTE y Dapper configurado para SQL Server.

## Características

- ASP.NET Core 6.0
- AdminLTE (incluido en wwwroot)
- Dapper para acceso a datos
- SQL Server con Microsoft.Data.SqlClient
- Patrón Repository implementado con ejemplos

## Configuración

1. Clona este repositorio
2. Actualiza el connection string en `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "SqlConnection": "server=YOUR_SERVER; database=YOUR_DATABASE; Integrated Security=true;encrypt=false;TrustServerCertificate=true"
   }
   ```
3. Ejecuta el proyecto:
   ```bash
   dotnet run --project FolderViewWeb
   ```

## Estructura del Proyecto

- **Controllers**: Controladores MVC (ejemplo: HomeController)
- **Dapper**: Configuración y repositorios de Dapper
  - **DapperContext**: Clase para crear conexiones a la base de datos
  - **Entidades**: Clases de dominio
  - **Interfaces**: Interfaces de repositorios
  - **Repositorios**: Implementaciones de repositorios
- **Views**: Vistas Razor (Home y Shared con layout AdminLTE)
- **wwwroot**: Archivos estáticos, incluyendo AdminLTE

## Uso de Dapper

El proyecto incluye ejemplos de repositorios con Dapper. Para agregar tus propios repositorios:

1. Crea tu entidad en `Dapper/Entidades`
2. Crea la interfaz en `Dapper/Interfaces`
3. Implementa el repositorio en `Dapper/Repositorios`
4. Registra el servicio en `Program.cs`:
   ```csharp
   builder.Services.AddScoped<ITuRepositorio, TuRepositorio>();
   ```

## Licencia

Este es un template libre para uso personal y comercial.
