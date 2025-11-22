using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalPOO2.Models;
using ProyectoFinalPOO2.Data;

namespace ProyectoFinalPOO2.Controllers
{
    public class MenuController : Controller
    {
        private readonly CafeteriaContext _context;
        
        public MenuController(CafeteriaContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            // Obtener productos disponibles de la base de datos con sus categorías
            var productos = await _context.Productos
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
            var productosAgrupados = productos
                .GroupBy(p => p.Categoria)
                .ToDictionary(g => g.Key ?? "Sin categoría", g => g.ToList());
            
            ViewBag.ProductosAgrupados = productosAgrupados;
            
            return View();
        }
    }
}
