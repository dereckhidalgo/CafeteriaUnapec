using CafeteriaUnapec.Data;
using CafeteriaUnapec.Model;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaUnapec.Routes
{
    public static class CampusRoute
    {
        public static void MapCampuRoutes(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/campus").WithTags("Campus");


            group.MapGet("", async (CafeteriaDbContext db) =>
                                await db.Campus.Where(x => x.Estado).ToListAsync()
            )
            .WithName("GetCampus")
            .WithOpenApi();

            group.MapPost("", async (Campus campus, CafeteriaDbContext db) =>
            {
                db.Campus.Add(campus);
                await db.SaveChangesAsync();
                return Results.Created($"/api/campus/{campus.Id}", campus);
            })
            .WithName("CreateCampus")
            .WithOpenApi();

            group.MapPut("{id:int}", async (int id, Campus input, CafeteriaDbContext db) =>
            {
                var campus = await db.Campus.FindAsync(id);
                if (campus is null) return Results.NotFound();

                campus.Descripcion = input.Descripcion;
                campus.Estado = input.Estado;
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateCampus")
            .WithOpenApi();
        }
    }
}
