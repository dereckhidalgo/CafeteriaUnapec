using CafeteriaUnapec.Data;
using CafeteriaUnapec.DTOs.UsuariosDTO;
using CafeteriaUnapec.Model;
using CafeteriaUnapec.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaUnapec.Routes
{
    public static class MarcaRoute
    {
        public static void MapMarcaRoutes(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/marcas").WithTags("Marcas");

            // GET: Obtener todos los tipos de usuarios activos
            group.MapGet("", async (CafeteriaDbContext db) =>
                        await db.Marcas.Where(x => x.Estado).ToListAsync())
                                                            .WithName("GetMarcas")
                                                            .WithOpenApi();

            group.MapPost("", async (Marca marca, CafeteriaDbContext db) =>
            {
                db.Marcas.Add(marca);
                await db.SaveChangesAsync();
                return Results.Created($"/api/marcas/{marca.Id}", marca);
            })
            .WithName("CreateMarca")
            .WithOpenApi();

            group.MapPut("{id:int}", async (int id, Marca input, CafeteriaDbContext db) =>
            {
                var marca = await db.Marcas.FindAsync(id);
                if (marca is null) return Results.NotFound();

                marca.Descripcion = input.Descripcion;
                marca.Estado = input.Estado;
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateMarca")
            .WithOpenApi();
        }
    }
}
