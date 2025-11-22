using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalPOO2.ViewModels
{
    public class RegistroViewModel
    {
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
        
        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; } = "";
        
        [Display(Name = "URL Foto de Perfil")]
        public string? FotoPerfil { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar un tipo de cuenta")]
        [Display(Name = "Tipo de Cuenta")]
        public string TipoCuenta { get; set; } = "Usuario";
    }
}
