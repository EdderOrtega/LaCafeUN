using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalPOO2.Entities
{
    [Table("Usuarios")]
    public class UsuarioEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = "";
        
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = "";
        
        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = "";
        
        [MaxLength(500)]
        public string? FotoPerfil { get; set; }
        
        public bool Activo { get; set; } = true;
        
        // Relación con Pedidos
        public virtual ICollection<PedidoEntity> Pedidos { get; set; } = new List<PedidoEntity>();
    }
}
