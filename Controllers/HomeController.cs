using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalPOO2.Models;
using ProyectoFinalPOO2.Data;

namespace ProyectoFinalPOO2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CafeteriaContext _context;

        public HomeController(ILogger<HomeController> logger, CafeteriaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Si el usuario está autenticado (usuario o admin), redirigir al dashboard
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var adminId = HttpContext.Session.GetInt32("AdminId");
            
            if (usuarioId != null || adminId != null)
            {
                return RedirectToAction("Dashboard");
            }
            
            // Mostrar landing page para visitantes
            // Obtener productos con sus categorías
            var productosEntities = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Disponible)
                .Select(e => new Producto
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    Precio = e.Precio,
                    Categoria = e.Categoria.Nombre,
                    Disponible = e.Disponible,
                    ImagenUrl = e.ImagenUrl
                })
                .ToListAsync();
            
            // Agrupar por categoría
            var productosAgrupados = productosEntities
                .GroupBy(p => p.Categoria)
                .ToDictionary(g => g.Key ?? "Sin categoría", g => g.Take(3).ToList());
            
            ViewBag.ProductosAgrupados = productosAgrupados;
            
            return View();
        }
        
        public IActionResult Dashboard()
        {
            // Verificar que el usuario o admin esté autenticado
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var adminId = HttpContext.Session.GetInt32("AdminId");
            
            if (usuarioId == null && adminId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            // Obtener el nombre según el tipo de cuenta
            var nombre = HttpContext.Session.GetString("UsuarioNombre") 
                      ?? HttpContext.Session.GetString("AdminNombre") 
                      ?? "Usuario";
            
            ViewBag.UsuarioNombre = nombre;
            ViewBag.EsAdmin = adminId != null;
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
