using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalPOO2.Models
{
    // ViewModel para usuarios (para las vistas)
    public class Usuario
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = "";
        
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email no válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";
        
        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = "";
        
        [Display(Name = "Foto de Perfil")]
        public string? FotoPerfil { get; set; }
        
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
