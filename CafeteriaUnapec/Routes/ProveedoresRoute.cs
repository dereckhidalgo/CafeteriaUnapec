using CafeteriaUnapec.Data;
using CafeteriaUnapec.Model;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaUnapec.Routes
{
    public static class ProveedoresRoute
    {
        public static void MapProveedoresRoutes(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/proveedores").WithTags("Proveedores");


            group.MapGet("", async (CafeteriaDbContext db) =>
                await db.Proveedores.Where(x => x.Estado).ToListAsync()
            )
            .WithName("GetProveedores")
            .WithOpenApi();



            group.MapPost("", async (Proveedor proveedor, CafeteriaDbContext db) =>
            {
                db.Proveedores.Add(proveedor);
                await db.SaveChangesAsync();
                return Results.Created($"/api/proveedores/{proveedor.Id}", proveedor);
            })
            .WithName("CreateProveedores")
            .WithOpenApi();

            group.MapPut("{id:int}", async (int id, Proveedor input, CafeteriaDbContext db) =>
            {
                var proveedor = await db.Proveedores.FindAsync(id);
                if (proveedor is null) return Results.NotFound();

                proveedor.NombreComercial = input.NombreComercial;
                proveedor.RNC = input.RNC;
                proveedor.Estado = input.Estado;
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateProveedores")
            .WithOpenApi();

        }
    }
}
