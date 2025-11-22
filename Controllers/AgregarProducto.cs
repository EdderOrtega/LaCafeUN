using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalPOO2.Models;
using ProyectoFinalPOO2.Data;
using ProyectoFinalPOO2.Entities;
using ProyectoFinalPOO2.Services;

namespace ProyectoFinalPOO2.Controllers
{
    public class AgregarProductoController : Controller
    {
        private readonly CafeteriaContext _context;
        private readonly IImagenService _imagenService;
        
        public AgregarProductoController(CafeteriaContext context, IImagenService imagenService)
        {
            _context = context;
            _imagenService = imagenService;
        }
        
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
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
            
            var categorias = await _context.CategoriasProducto
                .Where(c => c.Activo)
                .ToListAsync();
            
            ViewBag.Productos = productos;
            ViewBag.Categorias = categorias;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(Producto producto, int categoriaId, IFormFile? archivoImagen)
        {
            if (ModelState.IsValid || true)
            {
                string? urlImagen = null;

                // Subir imagen si se proporcionó
                if (archivoImagen != null && archivoImagen.Length > 0)
                {
                    urlImagen = await _imagenService.SubirImagenAsync(archivoImagen, "productos");
                    
                    if (string.IsNullOrEmpty(urlImagen))
                    {
                        TempData["Error"] = "Error al subir la imagen. Intenta de nuevo.";
                        return RedirectToAction("Index");
                    }
                }
                
                var productoEntity = new ProductoEntity
                {
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    CategoriaId = categoriaId,
                    Disponible = producto.Disponible,
                    ImagenUrl = urlImagen,
                    Stock = 100,
                    FechaCreacion = DateTime.UtcNow
                };
                
                _context.Productos.Add(productoEntity);
                await _context.SaveChangesAsync();
                
                TempData["Mensaje"] = "Producto agregado exitosamente!";
                return RedirectToAction("Index");
            }
            
            var productos = await _context.Productos
                .Include(p => p.Categoria)
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
            
            var categorias = await _context.CategoriasProducto
                .Where(c => c.Activo)
                .ToListAsync();
                
            ViewBag.Productos = productos;
            ViewBag.Categorias = categorias;
            
            return View(producto);
        }
        
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                // Eliminar imagen si existe y no es una URL externa
                if (!string.IsNullOrWhiteSpace(producto.ImagenUrl) && producto.ImagenUrl.StartsWith("/uploads/"))
                {
                    await _imagenService.EliminarImagenAsync(producto.ImagenUrl);
                }
                
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Producto eliminado exitosamente!";
            }
            return RedirectToAction("Index");
        }
    }
}
