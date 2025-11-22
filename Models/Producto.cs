using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalPOO2.Models
{
    // ViewModel para productos (para las vistas)
    public class Producto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre del Producto")]
        public string Nombre { get; set; } = "";
        
        [Required(ErrorMessage = "La descripción es requerida")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = "";
        
        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }
        
        [Required(ErrorMessage = "La categoría es requerida")]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; } = "";
        
        [Display(Name = "Disponible")]
        public bool Disponible { get; set; } = true;
        
        [Display(Name = "URL de Imagen")]
        public string? ImagenUrl { get; set; }
    }
}
