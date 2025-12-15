using FolderView.Dapper;
using FolderView.Dapper.Repositorios;
using FolderView.Dapper.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuración de Dapper Context
builder.Services.AddSingleton<DapperContext>();

// Ejemplo de repositorios (puedes eliminar o agregar los tuyos)
builder.Services.AddScoped<IDirectoryRepository, DirectorioRepositorio>();
builder.Services.AddScoped<IArchivoRepository, ArchivoRepositorio>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
