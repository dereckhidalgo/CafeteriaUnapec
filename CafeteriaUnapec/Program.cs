using CafeteriaUnapec.Routes;
using CafeteriaUnapec.Services.Interfaces;
using CafeteriaUnapec.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CafeteriaUnapec.Model;
using CafeteriaUnapec.Data;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddDbContext<CafeteriaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios
builder.Services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configurar pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// Crear la base de datos automáticamente
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CafeteriaDbContext>();
    await context.Database.EnsureCreatedAsync();
    await context.Database.MigrateAsync(); // Para aplicar migraciones
}

// ==================== ENDPOINTS TIPOS DE USUARIOS ====================
app.MapTipoUsuarioRoutes();

// ==================== ENDPOINTS CAMPUS ====================
app.MapCampuRoutes();

// ==================== ENDPOINTS MARCAS ====================
app.MapMarcaRoutes();


// ==================== ENDPOINTS PROVEEDORES ====================

app.MapProveedoresRoutes();

// ==================== ENDPOINTS CAFETERÍAS ====================
app.MapCafeteriasRoutes();

// ==================== ENDPOINTS USUARIOS ====================
app.MapUsuariosRoute();

// ==================== ENDPOINTS EMPLEADOS ====================
app.MapEmpleadosRoutes();

// ==================== ENDPOINTS ARTÍCULOS ====================
app.MapArticulosRoutes();

// ==================== ENDPOINTS VENTAS ====================
app.MapVentasRoutes();

// ==================== CONSULTAS Y REPORTES ====================
app.MapReportesRoutes();

app.Run();



public class Campus
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Descripcion { get; set; } = string.Empty;
    public bool Estado { get; set; } = true;

    // Navegación
    public virtual ICollection<Cafeteria> Cafeterias { get; set; } = new List<Cafeteria>();
}

public class Proveedor
{
    public int Id { get; set; }
    [Required, MaxLength(200)]
    public string NombreComercial { get; set; } = string.Empty;
    [Required, MaxLength(20)]
    public string RNC { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    public bool Estado { get; set; } = true;

    // Navegación
    public virtual ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
}

public class Cafeteria
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Descripcion { get; set; } = string.Empty;
    public int CampusId { get; set; }
    public int EncargadoId { get; set; }
    public bool Estado { get; set; } = true;

    // Navegación
    public virtual Campus Campus { get; set; } = null!;
    public virtual Empleado Encargado { get; set; } = null!;
}

public class Usuario
{
    public int Id { get; set; }
    [Required, MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;
    [Required, MaxLength(20)]
    public string Cedula { get; set; } = string.Empty;
    public int TipoUsuarioId { get; set; }
    public decimal LimiteCredito { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    public bool Estado { get; set; } = true;

    // Navegación
    public virtual TipoUsuario TipoUsuario { get; set; } = null!;
    public virtual ICollection<FacturacionArticulo> Compras { get; set; } = new List<FacturacionArticulo>();
}

public class Empleado
{
    public int Id { get; set; }
    [Required, MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;
    [Required, MaxLength(20)]
    public string Cedula { get; set; } = string.Empty;
    [Required, MaxLength(50)]
    public string TandaLabor { get; set; } = string.Empty;
    public decimal PorcientoComision { get; set; }
    public DateTime FechaIngreso { get; set; } = DateTime.Now;
    public bool Estado { get; set; } = true;

    // Navegación
    public virtual ICollection<Cafeteria> CafeteriasEncargadas { get; set; } = new List<Cafeteria>();
    public virtual ICollection<FacturacionArticulo> Ventas { get; set; } = new List<FacturacionArticulo>();
}

public class Articulo
{
    public int Id { get; set; }
    [Required, MaxLength(200)]
    public string Descripcion { get; set; } = string.Empty;
    public int MarcaId { get; set; }
    public decimal Costo { get; set; }
    public int ProveedorId { get; set; }
    public int Existencia { get; set; }
    public bool Estado { get; set; } = true;

    // Navegación
    public virtual Marca Marca { get; set; } = null!;
    public virtual Proveedor Proveedor { get; set; } = null!;
    public virtual ICollection<FacturacionArticulo> Ventas { get; set; } = new List<FacturacionArticulo>();
}

public class FacturacionArticulo
{
    public int NoFactura { get; set; }
    public int EmpleadoId { get; set; }
    public int ArticuloId { get; set; }
    public int UsuarioId { get; set; }
    public DateTime FechaVenta { get; set; } = DateTime.Now;
    public decimal MontoArticulo { get; set; }
    public int UnidadesVendidas { get; set; }
    [MaxLength(500)]
    public string? Comentario { get; set; }
    public bool Estado { get; set; } = true;

    // Navegación
    public virtual Empleado Empleado { get; set; } = null!;
    public virtual Articulo Articulo { get; set; } = null!;
    public virtual Usuario Usuario { get; set; } = null!;
}