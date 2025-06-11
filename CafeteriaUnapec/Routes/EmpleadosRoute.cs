using CafeteriaUnapec.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaUnapec.Routes
{
    public static class EmpleadosRoute
    {
        public static void MapEmpleadosRoutes(this WebApplication app)
        {
            var empleadosGroup = app.MapGroup("/api/empleados")
                .WithTags("Empleados");

            empleadosGroup.MapGet("/", async (CafeteriaDbContext db) =>
                await db.Empleados.Where(x => x.Estado).ToListAsync()
            )
            .WithName("GetEmpleados")
            .WithOpenApi();

            empleadosGroup.MapPost("/", async (Empleado empleado, CafeteriaDbContext db) =>
            {
                db.Empleados.Add(empleado);
                await db.SaveChangesAsync();
                return Results.Created($"/api/empleados/{empleado.Id}", empleado);
            })
            .WithName("CreateEmpleado")
            .WithOpenApi();

            empleadosGroup.MapPut("{id:int}", async (int id, Empleado input, CafeteriaDbContext db) =>
            {
                var empleado = await db.Empleados.FindAsync(id);
                if (empleado is null) return Results.NotFound();

                empleado.Nombre = input.Nombre;
                empleado.Cedula = input.Cedula;
                empleado.TandaLabor = input.TandaLabor;
                empleado.PorcientoComision = input.PorcientoComision;
                empleado.Estado = input.Estado;

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateEmpleado")
            .WithOpenApi();
        }
    }
}
