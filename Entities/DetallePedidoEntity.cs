using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalPOO2.Entities
{
    [Table("DetallesPedidos")]
    public class DetallePedidoEntity
    {
        [Key]
        public int Id { get; set; }
        
        public int Cantidad { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }
        
        [MaxLength(200)]
        public string? Notas { get; set; }
        
        // Claves foráneas
        public int PedidoId { get; set; }
        
        public int ProductoId { get; set; }
        
        // Relaciones
        [ForeignKey("PedidoId")]
        public PedidoEntity Pedido { get; set; } = null!;
        
        [ForeignKey("ProductoId")]
        public ProductoEntity Producto { get; set; } = null!;
    }
}
