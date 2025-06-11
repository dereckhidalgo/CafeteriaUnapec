using CafeteriaUnapec.DTOs.UsuariosDTO;
using CafeteriaUnapec.Services;
using CafeteriaUnapec.Services.Interfaces;

namespace CafeteriaUnapec.Routes
{
    public static class TipoUsuarioRoute
    {
        public static void MapTipoUsuarioRoutes(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/tipos-usuarios").WithTags("Tipos de Usuarios");

            // GET: Obtener todos los tipos de usuarios activos
            group.MapGet("", async (ITipoUsuarioService service) =>
            {
                var tiposUsuarios = await service.GetAllTiposUsuariosAsync();
                return Results.Ok(tiposUsuarios);
            })
            .WithName("GetTiposUsuarios")
            .WithOpenApi();

            // GET: Obtener tipo de usuario por ID
            group.MapGet("{id:int}", async (int id, ITipoUsuarioService service) =>
            {
                var tipoUsuario = await service.GetTipoUsuarioByIdAsync(id);
                return tipoUsuario is not null ? Results.Ok(tipoUsuario) : Results.NotFound();
            })
            .WithName("GetTipoUsuarioById")
            .WithOpenApi();

            // POST: Crear nuevo tipo de usuario
            group.MapPost("", async (TipoUsuarioCreateDto tipoUsuario, ITipoUsuarioService service) =>
            {
                var createdTipoUsuario = await service.CreateTipoUsuarioAsync(tipoUsuario);
                return Results.Created($"/api/tipos-usuarios/{createdTipoUsuario.Id}", createdTipoUsuario);
            })
            .WithName("CreateTipoUsuario")
            .WithOpenApi();

            // PUT: Actualizar tipo de usuario
            group.MapPut("{id:int}", async (int id, TipoUsuarioUpdateDto input, ITipoUsuarioService service) =>
            {
                var updated = await service.UpdateTipoUsuarioAsync(id, input);
                return updated ? Results.NoContent() : Results.NotFound();
            })
            .WithName("UpdateTipoUsuario")
            .WithOpenApi();

            // DELETE: Eliminar tipo de usuario (soft delete)
            group.MapDelete("{id:int}", async (int id, ITipoUsuarioService service) =>
            {
                var deleted = await service.DeleteTipoUsuarioAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteTipoUsuario")
            .WithOpenApi();
        }
    }
}
