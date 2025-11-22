using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalPOO2.Entities
{
    [Table("CategoriasProducto")]
    public class CategoriaProductoEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = "";
        
        [MaxLength(200)]
        public string? Descripcion { get; set; }
        
        [MaxLength(500)]
        public string? IconoUrl { get; set; }
        
        public bool Activo { get; set; } = true;
        
        // Relaciones
        public ICollection<ProductoEntity> Productos { get; set; } = new List<ProductoEntity>();
    }
}
