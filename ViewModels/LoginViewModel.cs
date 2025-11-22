using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalPOO2.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email no válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";
        
        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = "";
        
        [Required(ErrorMessage = "Debe seleccionar el tipo de cuenta")]
        [Display(Name = "Tipo de Cuenta")]
        public string TipoCuenta { get; set; } = "Usuario";
    }
}
