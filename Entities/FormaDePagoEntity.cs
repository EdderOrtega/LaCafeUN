using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalPOO2.Entities
{
    [Table("FormasDePago")]
    public class FormaDePagoEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = ""; // Efectivo, Tarjeta, Transferencia
        
        [MaxLength(200)]
        public string? Descripcion { get; set; }
        
        public bool Activo { get; set; } = true;
        
        // Relaciones
        public ICollection<PedidoEntity> Pedidos { get; set; } = new List<PedidoEntity>();
    }
}
