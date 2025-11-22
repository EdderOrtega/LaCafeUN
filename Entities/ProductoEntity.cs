using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalPOO2.Entities
{
    [Table("Productos")]
    public class ProductoEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = "";
        
        [MaxLength(500)]
        public string? Descripcion { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }
        
        [MaxLength(500)]
        public string? ImagenUrl { get; set; }
        
        public bool Disponible { get; set; } = true;
        
        public int Stock { get; set; } = 0;
        
        public DateTime FechaCreacion { get; set; }
        
        // Clave foránea
        public int CategoriaId { get; set; }
        
        // Relaciones
        [ForeignKey("CategoriaId")]
        public CategoriaProductoEntity Categoria { get; set; } = null!;
        
        public ICollection<DetallePedidoEntity> DetallesPedidos { get; set; } = new List<DetallePedidoEntity>();
    }
}
