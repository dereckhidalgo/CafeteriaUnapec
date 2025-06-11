using CafeteriaUnapec.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaUnapec.Routes
{
    public static class VentasRoute
    {
        public static void MapVentasRoutes(this WebApplication app)
        {
            var ventasGroup = app.MapGroup("/api/ventas")
                                 .WithTags("Ventas");

            ventasGroup.MapGet("/", async (CafeteriaDbContext db) =>
                await db.FacturacionArticulos
                    .Include(f => f.Empleado)
                    .Include(f => f.Articulo)
                    .Include(f => f.Usuario)
                    .Where(x => x.Estado)
                    .OrderByDescending(x => x.FechaVenta)
                    .ToListAsync()
            )
            .WithName("GetVentas")
            .WithOpenApi();

            ventasGroup.MapPost("/", async (FacturacionArticulo venta, CafeteriaDbContext db) =>
            {
                // Verificar existencia del artículo
                var articulo = await db.Articulos.FindAsync(venta.ArticuloId);
                if (articulo == null || articulo.Existencia < venta.UnidadesVendidas)
                    return Results.BadRequest("No hay suficiente existencia del artículo");

                // Reducir existencia
                articulo.Existencia -= venta.UnidadesVendidas;
                db.FacturacionArticulos.Add(venta);
                await db.SaveChangesAsync();

                return Results.Created($"/api/ventas/{venta.NoFactura}", venta);
            })
            .WithName("CreateVenta")
            .WithOpenApi();
        }
    }
}
