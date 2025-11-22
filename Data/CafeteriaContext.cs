using Microsoft.EntityFrameworkCore;
using ProyectoFinalPOO2.Entities;

namespace ProyectoFinalPOO2.Data
{
    public class CafeteriaContext : DbContext
    {
        public CafeteriaContext(DbContextOptions<CafeteriaContext> options) : base(options)
        {
        }

        // Tablas de la base de datos
        public DbSet<AdministradorEntity> Administradores { get; set; }
        public DbSet<UsuarioEntity> Usuarios { get; set; }
        public DbSet<CategoriaProductoEntity> CategoriasProducto { get; set; }
        public DbSet<ProductoEntity> Productos { get; set; }
        public DbSet<FormaDePagoEntity> FormasDePago { get; set; }
        public DbSet<PedidoEntity> Pedidos { get; set; }
        public DbSet<DetallePedidoEntity> DetallesPedidos { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configurar índices únicos
            modelBuilder.Entity<AdministradorEntity>()
                .HasIndex(a => a.Email)
                .IsUnique();
            
            modelBuilder.Entity<UsuarioEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();
            
            modelBuilder.Entity<PedidoEntity>()
                .HasIndex(p => p.NumeroPedido)
                .IsUnique();
            
            // Configurar relaciones
            modelBuilder.Entity<ProductoEntity>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<PedidoEntity>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<PedidoEntity>()
                .HasOne(p => p.FormaDePago)
                .WithMany(f => f.Pedidos)
                .HasForeignKey(p => p.FormaDePagoId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<DetallePedidoEntity>()
                .HasOne(d => d.Pedido)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<DetallePedidoEntity>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Datos semilla
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Categorías
            modelBuilder.Entity<CategoriaProductoEntity>().HasData(
                new CategoriaProductoEntity { Id = 1, Nombre = "Bebidas Calientes", Descripcion = "Café, té y bebidas calientes", IconoUrl = "☕" },
                new CategoriaProductoEntity { Id = 2, Nombre = "Bebidas Frías", Descripcion = "Jugos, smoothies y bebidas frías", IconoUrl = "🥤" },
                new CategoriaProductoEntity { Id = 3, Nombre = "Comida", Descripcion = "Platillos y alimentos", IconoUrl = "🍔" },
                new CategoriaProductoEntity { Id = 4, Nombre = "Postres", Descripcion = "Pasteles y postres", IconoUrl = "🍰" },
                new CategoriaProductoEntity { Id = 5, Nombre = "Snacks", Descripcion = "Botanas y snacks", IconoUrl = "🍿" }
            );
            
            // Formas de pago
            modelBuilder.Entity<FormaDePagoEntity>().HasData(
                new FormaDePagoEntity { Id = 1, Nombre = "Efectivo", Descripcion = "Pago en efectivo" },
                new FormaDePagoEntity { Id = 2, Nombre = "Tarjeta", Descripcion = "Tarjeta de débito o crédito" },
                new FormaDePagoEntity { Id = 3, Nombre = "Transferencia", Descripcion = "Transferencia bancaria" }
            );
            
            // Administrador por defecto
            modelBuilder.Entity<AdministradorEntity>().HasData(
                new AdministradorEntity 
                { 
                    Id = 1, 
                    Nombre = "Admin La Cafe", 
                    Email = "admin@lacafe.com", 
                    Password = "admin123",
                    FechaCreacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Activo = true
                }
            );
        }
    }
}
