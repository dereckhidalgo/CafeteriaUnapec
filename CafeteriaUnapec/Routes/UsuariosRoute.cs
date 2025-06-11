using CafeteriaUnapec.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaUnapec.Routes
{
    public static class UsuariosRoute
    {
        public static void MapUsuariosRoute(this WebApplication app)
        {
            var usuariosGroup = app.MapGroup("/api/usuarios")
                .WithTags("Usuarios");

            usuariosGroup.MapGet("/", async (CafeteriaDbContext db) =>
                await db.Usuarios.Include(u => u.TipoUsuario)
                    .Where(x => x.Estado).ToListAsync()
            )
            .WithName("GetUsuarios")
            .WithOpenApi();

            usuariosGroup.MapPost("/", async (Usuario usuario, CafeteriaDbContext db) =>
            {
                db.Usuarios.Add(usuario);
                await db.SaveChangesAsync();
                return Results.Created($"/api/usuarios/{usuario.Id}", usuario);
            })
            .WithName("CreateUsuario")
            .WithOpenApi();

            usuariosGroup.MapPut("{id:int}", async (int id, Usuario input, CafeteriaDbContext db) =>
            {
                var usuario = await db.Usuarios.FindAsync(id);
                if (usuario is null) return Results.NotFound();

                usuario.Nombre = input.Nombre;
                usuario.Cedula = input.Cedula;
                usuario.TipoUsuarioId = input.TipoUsuarioId;
                usuario.LimiteCredito = input.LimiteCredito;
                usuario.Estado = input.Estado;

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateUsuario")
            .WithOpenApi();
        }
    }
}
