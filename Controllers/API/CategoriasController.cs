using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalPOO2.Data;

namespace ProyectoFinalPOO2.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly CafeteriaContext _context;

        public CategoriasController(CafeteriaContext context)
        {
            _context = context;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCategorias()
        {
            var categorias = await _context.CategoriasProducto
                .Where(c => c.Activo)
                .Select(c => new
                {
                    c.Id,
                    c.Nombre,
                    c.Descripcion,
                    c.IconoUrl,
                    TotalProductos = c.Productos.Count(p => p.Disponible)
                })
                .ToListAsync();

            return Ok(categorias);
        }

        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCategoria(int id)
        {
            var categoria = await _context.CategoriasProducto
                .Where(c => c.Id == id && c.Activo)
                .Select(c => new
                {
                    c.Id,
                    c.Nombre,
                    c.Descripcion,
                    c.IconoUrl,
                    Productos = c.Productos.Where(p => p.Disponible).Select(p => new
                    {
                        p.Id,
                        p.Nombre,
                        p.Precio,
                        p.ImagenUrl
                    })
                })
                .FirstOrDefaultAsync();

            if (categoria == null)
            {
                return NotFound(new { message = "Categoría no encontrada" });
            }

            return Ok(categoria);
        }
    }
}
