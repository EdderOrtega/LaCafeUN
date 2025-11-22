using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalPOO2.Data;

namespace ProyectoFinalPOO2.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly CafeteriaContext _context;

        public ProductosController(CafeteriaContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProductos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Disponible)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Descripcion,
                    p.Precio,
                    p.ImagenUrl,
                    p.Stock,
                    p.Disponible,
                    Categoria = new
                    {
                        p.Categoria.Id,
                        p.Categoria.Nombre,
                        p.Categoria.IconoUrl
                    }
                })
                .ToListAsync();

            return Ok(productos);
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Descripcion,
                    p.Precio,
                    p.ImagenUrl,
                    p.Stock,
                    p.Disponible,
                    Categoria = new
                    {
                        p.Categoria.Id,
                        p.Categoria.Nombre,
                        p.Categoria.Descripcion,
                        p.Categoria.IconoUrl
                    }
                })
                .FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            return Ok(producto);
        }

        // GET: api/Productos/categoria/1
        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetProductosPorCategoria(int categoriaId)
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == categoriaId && p.Disponible)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Descripcion,
                    p.Precio,
                    p.ImagenUrl,
                    p.Stock,
                    Categoria = new
                    {
                        p.Categoria.Id,
                        p.Categoria.Nombre
                    }
                })
                .ToListAsync();

            return Ok(productos);
        }

        // GET: api/Productos/buscar?termino=cafe
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<object>>> BuscarProductos([FromQuery] string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
            {
                return BadRequest(new { message = "El término de búsqueda es requerido" });
            }

            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Disponible && (
                    p.Nombre.Contains(termino) ||
                    p.Descripcion!.Contains(termino) ||
                    p.Categoria.Nombre.Contains(termino)
                ))
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Descripcion,
                    p.Precio,
                    p.ImagenUrl,
                    Categoria = p.Categoria.Nombre
                })
                .ToListAsync();

            return Ok(productos);
        }
    }
}
