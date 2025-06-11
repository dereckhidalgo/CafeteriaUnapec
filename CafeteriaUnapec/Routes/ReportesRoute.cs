using CafeteriaUnapec.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaUnapec.Routes
{
    public static class ReportesRoute
    {
        public static void MapReportesRoutes(this WebApplication app)
        {
            var reportesGroup = app.MapGroup("/api/reportes")
                                   .WithTags("Reportes");

            reportesGroup.MapGet("/ventas-por-usuario/{usuarioId}", async (int usuarioId, CafeteriaDbContext db) =>
            {
                var ventas = await db.FacturacionArticulos
                    .Include(f => f.Articulo)
                    .Include(f => f.Usuario)
                    .Where(f => f.UsuarioId == usuarioId && f.Estado)
                    .ToListAsync();

                return Results.Ok(new
                {
                    Usuario = ventas.FirstOrDefault()?.Usuario?.Nombre,
                    TotalVentas = ventas.Count,
                    MontoTotal = ventas.Sum(v => v.MontoArticulo * v.UnidadesVendidas),
                    Ventas = ventas
                });
            })
            .WithName("GetVentasPorUsuario")
            .WithOpenApi();

            reportesGroup.MapGet("/ventas-por-fecha", async (DateTime fechaInicio, DateTime fechaFin, CafeteriaDbContext db) =>
            {
                var ventas = await db.FacturacionArticulos
                    .Include(f => f.Articulo)
                    .Include(f => f.Usuario)
                    .Include(f => f.Empleado)
                    .Where(f => f.FechaVenta >= fechaInicio && f.FechaVenta <= fechaFin && f.Estado)
                    .ToListAsync();

                return Results.Ok(new
                {
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    TotalVentas = ventas.Count,
                    MontoTotal = ventas.Sum(v => v.MontoArticulo * v.UnidadesVendidas),
                    Ventas = ventas
                });
            })
            .WithName("GetVentasPorFecha")
            .WithOpenApi();

            reportesGroup.MapGet("/ventas-por-proveedor/{proveedorId}", async (int proveedorId, CafeteriaDbContext db) =>
            {
                var ventas = await db.FacturacionArticulos
                    .Include(f => f.Articulo)
                    .ThenInclude(a => a.Proveedor)
                    .Where(f => f.Articulo.ProveedorId == proveedorId && f.Estado)
                    .ToListAsync();

                return Results.Ok(new
                {
                    Proveedor = ventas.FirstOrDefault()?.Articulo?.Proveedor?.NombreComercial,
                    TotalVentas = ventas.Count,
                    MontoTotal = ventas.Sum(v => v.MontoArticulo * v.UnidadesVendidas),
                    Ventas = ventas
                });
            })
            .WithName("GetVentasPorProveedor")
            .WithOpenApi();
        }
    }

}
