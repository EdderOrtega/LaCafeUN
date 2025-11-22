using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalPOO2.Models
{
    // ViewModel para pedidos (para las vistas)
    public class Pedido
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre del cliente es requerido")]
        [Display(Name = "Nombre del Cliente")]
        public string NombreCliente { get; set; } = "";
        
        [Display(Name = "Número de Mesa")]
        public string? NumeroMesa { get; set; }
        
        [Required(ErrorMessage = "El teléfono es requerido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = "";
        
        [Display(Name = "Fecha del Pedido")]
        public DateTime FechaPedido { get; set; } = DateTime.Now;
        
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Pendiente";
        
        [Display(Name = "Total")]
        public decimal Total { get; set; }
        
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }
        
        // Para mostrar los detalles
        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }
    
    public class DetallePedido
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
