using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalPOO2.Data;
using ProyectoFinalPOO2.Entities;

namespace ProyectoFinalPOO2.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly CafeteriaContext _context;

        public PedidosController(CafeteriaContext context)
        {
            _context = context;
        }

        // GET: api/Pedidos/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPedidosUsuario(int usuarioId)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(p => p.FormaDePago)
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.FechaPedido)
                .Select(p => new
                {
                    p.Id,
                    p.NumeroPedido,
                    p.Estado,
                    p.Total,
                    p.FechaPedido,
                    p.FechaEntrega,
                    p.NumeroMesa,
                    FormaDePago = p.FormaDePago.Nombre,
                    Detalles = p.Detalles.Select(d => new
                    {
                        d.Id,
                        Producto = d.Producto.Nombre,
                        d.Cantidad,
                        d.PrecioUnitario,
                        d.Subtotal,
                        ImagenProducto = d.Producto.ImagenUrl
                    })
                })
                .ToListAsync();

            return Ok(pedidos);
        }

        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(p => p.FormaDePago)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.NumeroPedido,
                    p.Estado,
                    p.Subtotal,
                    p.Impuesto,
                    p.Total,
                    p.FechaPedido,
                    p.FechaEntrega,
                    p.NumeroMesa,
                    p.Observaciones,
                    Usuario = new
                    {
                        p.Usuario.Id,
                        p.Usuario.Nombre,
                        p.Usuario.Email
                    },
                    FormaDePago = p.FormaDePago.Nombre,
                    Detalles = p.Detalles.Select(d => new
                    {
                        d.Id,
                        ProductoId = d.Producto.Id,
                        Producto = d.Producto.Nombre,
                        d.Cantidad,
                        d.PrecioUnitario,
                        d.Subtotal,
                        d.Notas,
                        ImagenProducto = d.Producto.ImagenUrl
                    })
                })
                .FirstOrDefaultAsync();

            if (pedido == null)
            {
                return NotFound(new { message = "Pedido no encontrado" });
            }

            return Ok(pedido);
        }

        // POST: api/Pedidos
        [HttpPost]
        public async Task<ActionResult<object>> CrearPedido([FromBody] CrearPedidoRequest request)
        {
            // Validar usuario
            var usuario = await _context.Usuarios.FindAsync(request.UsuarioId);
            if (usuario == null || !usuario.Activo)
            {
                return BadRequest(new { message = "Usuario no válido" });
            }

            // Validar forma de pago
            var formaPago = await _context.FormasDePago.FindAsync(request.FormaDePagoId);
            if (formaPago == null || !formaPago.Activo)
            {
                return BadRequest(new { message = "Forma de pago no válida" });
            }

            // Generar número de pedido único
            var numeroPedido = $"PED-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";

            // Crear pedido
            var pedido = new PedidoEntity
            {
                NumeroPedido = numeroPedido,
                UsuarioId = request.UsuarioId,
                FormaDePagoId = request.FormaDePagoId,
                NumeroMesa = request.NumeroMesa,
                Observaciones = request.Observaciones,
                Estado = "Pendiente",
                FechaPedido = DateTime.UtcNow,
                Detalles = new List<DetallePedidoEntity>()
            };

            decimal subtotal = 0;

            // Agregar detalles
            foreach (var detalle in request.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                if (producto == null || !producto.Disponible)
                {
                    return BadRequest(new { message = $"Producto {detalle.ProductoId} no disponible" });
                }

                // Verificar stock
                if (producto.Stock < detalle.Cantidad)
                {
                    return BadRequest(new { message = $"Stock insuficiente para {producto.Nombre}" });
                }

                var detallePedido = new DetallePedidoEntity
                {
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = producto.Precio,
                    Subtotal = producto.Precio * detalle.Cantidad,
                    Notas = detalle.Notas
                };

                pedido.Detalles.Add(detallePedido);
                subtotal += detallePedido.Subtotal;

                // Actualizar stock
                producto.Stock -= detalle.Cantidad;
            }

            // Calcular totales
            pedido.Subtotal = subtotal;
            pedido.Impuesto = subtotal * 0.16m; // 16% IVA
            pedido.Total = pedido.Subtotal + pedido.Impuesto;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, new
            {
                pedido.Id,
                pedido.NumeroPedido,
                pedido.Total,
                pedido.Estado,
                message = "Pedido creado exitosamente"
            });
        }

        // PUT: api/Pedidos/5/estado
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] ActualizarEstadoRequest request)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound(new { message = "Pedido no encontrado" });
            }

            var estadosValidos = new[] { "Pendiente", "EnPreparacion", "Listo", "Entregado", "Cancelado" };
            if (!estadosValidos.Contains(request.Estado))
            {
                return BadRequest(new { message = "Estado no válido" });
            }

            pedido.Estado = request.Estado;

            if (request.Estado == "Entregado")
            {
                pedido.FechaEntrega = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Estado actualizado a {request.Estado}" });
        }

        // GET: api/Pedidos/pendientes
        [HttpGet("pendientes")]
        public async Task<ActionResult<IEnumerable<object>>> GetPedidosPendientes()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Where(p => p.Estado == "Pendiente" || p.Estado == "EnPreparacion")
                .OrderBy(p => p.FechaPedido)
                .Select(p => new
                {
                    p.Id,
                    p.NumeroPedido,
                    p.Estado,
                    p.Total,
                    p.FechaPedido,
                    p.NumeroMesa,
                    Usuario = p.Usuario.Nombre,
                    TotalItems = p.Detalles.Sum(d => d.Cantidad)
                })
                .ToListAsync();

            return Ok(pedidos);
        }

        // DELETE: api/Pedidos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound(new { message = "Pedido no encontrado" });
            }

            // Solo se pueden cancelar pedidos pendientes
            if (pedido.Estado != "Pendiente")
            {
                return BadRequest(new { message = "Solo se pueden cancelar pedidos pendientes" });
            }

            // Restaurar stock
            foreach (var detalle in pedido.Detalles)
            {
                detalle.Producto.Stock += detalle.Cantidad;
            }

            pedido.Estado = "Cancelado";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pedido cancelado exitosamente" });
        }

        // Modelos de Request
        public class CrearPedidoRequest
        {
            public int UsuarioId { get; set; }
            public int FormaDePagoId { get; set; }
            public string? NumeroMesa { get; set; }
            public string? Observaciones { get; set; }
            public List<DetallePedidoRequest> Detalles { get; set; } = new();
        }

        public class DetallePedidoRequest
        {
            public int ProductoId { get; set; }
            public int Cantidad { get; set; }
            public string? Notas { get; set; }
        }

        public class ActualizarEstadoRequest
        {
            public string Estado { get; set; } = "";
        }
    }
}
