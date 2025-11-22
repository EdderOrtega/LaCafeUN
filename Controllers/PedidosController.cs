using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalPOO2.Data;
using ProyectoFinalPOO2.Entities;

namespace ProyectoFinalPOO2.Controllers
{
    public class PedidosController : Controller
    {
        private readonly CafeteriaContext _context;
        
        public PedidosController(CafeteriaContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            // Verificar si hay usuario en sesión
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
            {
                TempData["Mensaje"] = "Debes iniciar sesión para ver tus pedidos";
                return RedirectToAction("Login", "Account");
            }

            // Obtener pedidos del usuario
            var pedidos = await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(p => p.FormaDePago)
                .Where(p => p.UsuarioId == usuarioId.Value)
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();

            ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            return View(pedidos);
        }
    }
}
