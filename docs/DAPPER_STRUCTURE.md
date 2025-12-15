# Estructura de la Carpeta Dapper

Este documento explica cómo organizar y crear archivos para trabajar con Dapper en este template.

## ¿Para qué sirve la carpeta Dapper?

La carpeta `Dapper` contiene toda la lógica de acceso a datos usando el micro-ORM Dapper. Esta carpeta organiza las entidades, repositorios e interfaces necesarias para interactuar con la base de datos SQL Server.

## Estructura Organizativa

A diferencia de la organización tradicional por tipo (Entidades/, Interfaces/, Repositorios/), este template organiza los archivos **por entidad de base de datos**. Cada tabla de la base de datos tiene su propia carpeta con todos sus archivos relacionados.

### Estructura de carpetas:

```
FolderViewWeb/
└── Dapper/
    ├── DapperContext.cs          # Configuración de conexión a BD
    ├── Usuario/                   # Carpeta por cada tabla/entidad
    │   ├── UsuarioEntidad.cs     # Clase de dominio
    │   ├── IUsuarioRepository.cs # Interfaz del repositorio
    │   └── UsuarioRepositorio.cs # Implementación del repositorio
    ├── Producto/
    │   ├── ProductoEntidad.cs
    │   ├── IProductoRepository.cs
    │   └── ProductoRepositorio.cs
    └── ... (más entidades)
```

## Cómo crear archivos para una nueva entidad

Supongamos que tienes una tabla llamada `Clientes` en tu base de datos.

### 1. Crear la carpeta de la entidad

```bash
mkdir FolderViewWeb/Dapper/Cliente
```

### 2. Crear la clase Entidad

**Archivo:** `FolderViewWeb/Dapper/Cliente/ClienteEntidad.cs`

```csharp
namespace FolderView.Dapper.Cliente
{
    public class ClienteEntidad
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
    }
}
```

**Notas:**
- Nombra la clase como `[NombreEntidad]Entidad`
- Las propiedades deben coincidir con las columnas de la tabla
- Usa tipos nullable (`?`) para columnas que permiten NULL

### 3. Crear la interfaz del repositorio

**Archivo:** `FolderViewWeb/Dapper/Cliente/IClienteRepository.cs`

```csharp
namespace FolderView.Dapper.Cliente.Interfaces
{
    public interface IClienteRepository
    {
        // Operaciones CRUD básicas
        Task<IEnumerable<ClienteEntidad>> GetAllAsync();
        Task<ClienteEntidad?> GetByIdAsync(int id);
        Task<int> CreateAsync(ClienteEntidad cliente);
        Task<bool> UpdateAsync(ClienteEntidad cliente);
        Task<bool> DeleteAsync(int id);

        // Métodos personalizados según necesidades
        Task<IEnumerable<ClienteEntidad>> GetActivosAsync();
        Task<ClienteEntidad?> GetByEmailAsync(string email);
    }
}
```

**Notas:**
- Define los métodos que necesitarás para acceder a los datos
- Usa `async/await` para operaciones asíncronas
- Incluye CRUD básico y métodos específicos de negocio

### 4. Implementar el repositorio

**Archivo:** `FolderViewWeb/Dapper/Cliente/ClienteRepositorio.cs`

```csharp
using Dapper;
using System.Data;

namespace FolderView.Dapper.Cliente.Repositorios
{
    public class ClienteRepositorio : IClienteRepository
    {
        private readonly DapperContext _context;

        public ClienteRepositorio(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClienteEntidad>> GetAllAsync()
        {
            var query = "SELECT * FROM Clientes";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<ClienteEntidad>(query);
            }
        }

        public async Task<ClienteEntidad?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Clientes WHERE ClienteId = @Id";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<ClienteEntidad>(
                    query,
                    new { Id = id }
                );
            }
        }

        public async Task<int> CreateAsync(ClienteEntidad cliente)
        {
            var query = @"
                INSERT INTO Clientes (Nombre, Email, Telefono, FechaRegistro, Activo)
                VALUES (@Nombre, @Email, @Telefono, @FechaRegistro, @Activo);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(query, cliente);
            }
        }

        public async Task<bool> UpdateAsync(ClienteEntidad cliente)
        {
            var query = @"
                UPDATE Clientes
                SET Nombre = @Nombre,
                    Email = @Email,
                    Telefono = @Telefono,
                    Activo = @Activo
                WHERE ClienteId = @ClienteId";

            using (var connection = _context.CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync(query, cliente);
                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Clientes WHERE ClienteId = @Id";

            using (var connection = _context.CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
                return affectedRows > 0;
            }
        }

        public async Task<IEnumerable<ClienteEntidad>> GetActivosAsync()
        {
            var query = "SELECT * FROM Clientes WHERE Activo = 1";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<ClienteEntidad>(query);
            }
        }

        public async Task<ClienteEntidad?> GetByEmailAsync(string email)
        {
            var query = "SELECT * FROM Clientes WHERE Email = @Email";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<ClienteEntidad>(
                    query,
                    new { Email = email }
                );
            }
        }
    }
}
```

**Notas:**
- Inyecta `DapperContext` en el constructor
- Usa `using` para asegurar que las conexiones se cierren
- Usa parámetros (`@`) para prevenir SQL injection
- `QueryAsync<T>`: Devuelve múltiples registros
- `QuerySingleOrDefaultAsync<T>`: Devuelve un registro o null
- `ExecuteAsync`: Para INSERT, UPDATE, DELETE (devuelve filas afectadas)

### 5. Registrar el repositorio en Program.cs

Abre `Program.cs` y agrega:

```csharp
using FolderView.Dapper.Cliente.Interfaces;
using FolderView.Dapper.Cliente.Repositorios;

// ... código existente ...

builder.Services.AddScoped<IClienteRepository, ClienteRepositorio>();
```

## Convenciones de Nomenclatura

| Tipo | Nombre | Ejemplo |
|------|--------|---------|
| Carpeta | Nombre de la entidad en singular | `Cliente/`, `Producto/` |
| Entidad | `[Nombre]Entidad.cs` | `ClienteEntidad.cs` |
| Interfaz | `I[Nombre]Repository.cs` | `IClienteRepository.cs` |
| Repositorio | `[Nombre]Repositorio.cs` | `ClienteRepositorio.cs` |

## Ventajas de esta estructura

1. **Cohesión**: Todo lo relacionado a una entidad está en un mismo lugar
2. **Escalabilidad**: Fácil agregar nuevas entidades sin modificar las existentes
3. **Mantenibilidad**: Cambios en una entidad no afectan otras carpetas
4. **Navegación**: Más fácil encontrar archivos relacionados

## Usando Stored Procedures

Si prefieres usar Stored Procedures:

```csharp
public async Task<IEnumerable<ClienteEntidad>> GetActivosAsync()
{
    using (var connection = _context.CreateConnection())
    {
        return await connection.QueryAsync<ClienteEntidad>(
            "sp_GetClientesActivos",
            commandType: CommandType.StoredProcedure
        );
    }
}
```

## Relaciones entre entidades

Para consultas con joins o relaciones:

```csharp
public async Task<IEnumerable<PedidoConClienteDto>> GetPedidosConClienteAsync()
{
    var query = @"
        SELECT
            p.PedidoId,
            p.Fecha,
            p.Total,
            c.ClienteId,
            c.Nombre as ClienteNombre
        FROM Pedidos p
        INNER JOIN Clientes c ON p.ClienteId = c.ClienteId";

    using (var connection = _context.CreateConnection())
    {
        return await connection.QueryAsync<PedidoConClienteDto>(query);
    }
}
```

## Recursos adicionales

- [Dapper Documentation](https://github.com/DapperLib/Dapper)
- [Dapper Tutorial](https://www.learndapper.com/)
- [Best Practices](https://github.com/DapperLib/Dapper#performance)
