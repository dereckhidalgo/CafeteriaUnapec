using CafeteriaUnapec.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaUnapec.Routes
{
    public static class ArticulosRoute
    {

        public static void MapArticulosRoutes(this WebApplication app)
        {
            var articulosGroup = app.MapGroup("/api/articulos")
                .WithTags("Articulos");

            articulosGroup.MapGet("/", async (CafeteriaDbContext db) =>
                await db.Articulos.Include(a => a.Marca).Include(a => a.Proveedor)
                    .Where(x => x.Estado).ToListAsync())
                .WithName("GetArticulos")
                .WithOpenApi();

            articulosGroup.MapPost("/", async (Articulo articulo, CafeteriaDbContext db) =>
            {
                db.Articulos.Add(articulo);
                await db.SaveChangesAsync();
                return Results.Created($"/api/articulos/{articulo.Id}", articulo);
            })
            .WithName("CreateArticulo")
            .WithOpenApi();

            articulosGroup.MapPut("{id}", async (int id, Articulo input, CafeteriaDbContext db) =>
            {
                var articulo = await db.Articulos.FindAsync(id);
                if (articulo is null) return Results.NotFound();

                articulo.Descripcion = input.Descripcion;
                articulo.MarcaId = input.MarcaId;
                articulo.Costo = input.Costo;
                articulo.ProveedorId = input.ProveedorId;
                articulo.Existencia = input.Existencia;
                articulo.Estado = input.Estado;

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateArticulo")
            .WithOpenApi();
        }
    }
}
