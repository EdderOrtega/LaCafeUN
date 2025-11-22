using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalPOO2.Entities
{
    [Table("Pedidos")]
    public class PedidoEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string NumeroPedido { get; set; } = "";
        
        // Clave foránea
        public int UsuarioId { get; set; }
        
        public int FormaDePagoId { get; set; }
        
        [MaxLength(20)]
        public string? NumeroMesa { get; set; }
        
        [MaxLength(500)]
        public string? Observaciones { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Estado { get; set; } = "Pendiente";
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Impuesto { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }
        
        public DateTime FechaPedido { get; set; }
        
        public DateTime? FechaEntrega { get; set; } // OPCIONAL - se llena solo cuando se entrega
        
        // Relaciones
        [ForeignKey("UsuarioId")]
        public UsuarioEntity Usuario { get; set; } = null!;
        
        [ForeignKey("FormaDePagoId")]
        public FormaDePagoEntity FormaDePago { get; set; } = null!;
        
        public ICollection<DetallePedidoEntity> Detalles { get; set; } = new List<DetallePedidoEntity>();
    }
}
