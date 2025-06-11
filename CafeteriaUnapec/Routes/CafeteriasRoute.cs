using CafeteriaUnapec.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaUnapec.Routes
{
    public static class CafeteriasRoute
    {

        public static void MapCafeteriasRoutes(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/cafeterias").WithTags("cafeterias");


            group.MapGet("", async (CafeteriaDbContext db) =>
                await db.Cafeterias.Include(c => c.Campus).Include(c => c.Encargado)
                                                          .Where(x => x.Estado).ToListAsync()
            )
            .WithName("GetCafeterias")
            .WithOpenApi();

            group.MapPost("", async (Cafeteria cafeteria, CafeteriaDbContext db) =>
            {
                db.Cafeterias.Add(cafeteria);
                await db.SaveChangesAsync();
                return Results.Created($"/api/cafeterias/{cafeteria.Id}", cafeteria);
            })
            .WithName("CreateCafeterias")
            .WithOpenApi();

            group.MapPut("{id}", async (int id, Cafeteria input, CafeteriaDbContext db) =>
            {
                var cafeteria = await db.Cafeterias.FindAsync(id);
                if (cafeteria is null) return Results.NotFound();

                cafeteria.Descripcion = input.Descripcion;
                cafeteria.CampusId = input.CampusId;
                cafeteria.EncargadoId = input.EncargadoId;
                cafeteria.Estado = input.Estado;
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateCafeterias")
            .WithOpenApi();
        }
    }
}
