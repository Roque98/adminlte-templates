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
- **Dapper**: Configuración y repositorios de acceso a datos
  - **DapperContext.cs**: Clase para crear conexiones a la base de datos
  - Cada tabla tiene su propia carpeta con:
    - Entidad (modelo de datos)
    - Interfaz del repositorio
    - Implementación del repositorio
- **Views**: Vistas Razor (Home y Shared con layout AdminLTE)
- **wwwroot**: Archivos estáticos, incluyendo AdminLTE
- **docs**: Documentación del proyecto

## Uso de Dapper

Este template organiza los archivos de Dapper **por entidad** en lugar de por tipo. Cada tabla de la base de datos tiene su propia carpeta.

### Estructura ejemplo:
```
Dapper/
├── DapperContext.cs
├── Cliente/
│   ├── ClienteEntidad.cs
│   ├── IClienteRepository.cs
│   └── ClienteRepositorio.cs
└── Producto/
    ├── ProductoEntidad.cs
    ├── IProductoRepository.cs
    └── ProductoRepositorio.cs
```

### Pasos para agregar una nueva entidad:

1. Crea una carpeta para tu entidad: `Dapper/NombreEntidad/`
2. Crea la clase entidad: `NombreEntidadEntidad.cs`
3. Crea la interfaz del repositorio: `INombreEntidadRepository.cs`
4. Implementa el repositorio: `NombreEntidadRepositorio.cs`
5. Registra el servicio en `Program.cs`:
   ```csharp
   builder.Services.AddScoped<IClienteRepository, ClienteRepositorio>();
   ```

📖 **Para más detalles, consulta la documentación completa en:** [`docs/DAPPER_STRUCTURE.md`](docs/DAPPER_STRUCTURE.md)

## Licencia

Este es un template libre para uso personal y comercial.
