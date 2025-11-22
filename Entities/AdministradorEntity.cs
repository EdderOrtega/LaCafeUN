using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalPOO2.Entities
{
    [Table("Administradores")]
    public class AdministradorEntity
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
        
        public DateTime FechaCreacion { get; set; }
        
        public bool Activo { get; set; } = true;
    }
}
