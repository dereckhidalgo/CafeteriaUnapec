using CafeteriaUnapec.Model;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaUnapec.Data
{
    public class CafeteriaDbContext : DbContext
    {
        public CafeteriaDbContext(DbContextOptions<CafeteriaDbContext> options) : base(options) { }

        public DbSet<TipoUsuario> TiposUsuarios { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Campus> Campus { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Cafeteria> Cafeterias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<FacturacionArticulo> FacturacionArticulos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar TipoUsuario
            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(100);
            });

            // Configurar Marca
            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(100);
            });

            // Configurar Campus
            modelBuilder.Entity<Campus>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(100);
            });

            // Configurar Proveedor
            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NombreComercial).IsRequired().HasMaxLength(200);
                entity.Property(e => e.RNC).IsRequired().HasMaxLength(20);
            });

            // Configurar Cafeteria
            modelBuilder.Entity<Cafeteria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.Campus)
                      .WithMany(c => c.Cafeterias)
                      .HasForeignKey(e => e.CampusId);

                entity.HasOne(e => e.Encargado)
                      .WithMany(emp => emp.CafeteriasEncargadas)
                      .HasForeignKey(e => e.EncargadoId);
            });

            // Configurar Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Cedula).IsRequired().HasMaxLength(20);
                entity.Property(e => e.LimiteCredito).HasColumnType("decimal(18,2)");
            });

            // Configurar Empleado
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Cedula).IsRequired().HasMaxLength(20);
                entity.Property(e => e.TandaLabor).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PorcientoComision).HasColumnType("decimal(5,2)");
            });

            // Configurar Articulo
            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Costo).HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Marca)
                      .WithMany(m => m.Articulos)
                      .HasForeignKey(e => e.MarcaId);

                entity.HasOne(e => e.Proveedor)
                      .WithMany(p => p.Articulos)
                      .HasForeignKey(e => e.ProveedorId);
            });

            // Configurar FacturacionArticulo
            modelBuilder.Entity<FacturacionArticulo>(entity =>
            {
                entity.HasKey(e => e.NoFactura);
                entity.Property(e => e.MontoArticulo).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Comentario).HasMaxLength(500);

                entity.HasOne(e => e.Empleado)
                      .WithMany(emp => emp.Ventas)
                      .HasForeignKey(e => e.EmpleadoId);

                entity.HasOne(e => e.Articulo)
                      .WithMany(a => a.Ventas)
                      .HasForeignKey(e => e.ArticuloId);

                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Compras)
                      .HasForeignKey(e => e.UsuarioId);
            });
        }
    }
}
