using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalPOO2.Data;
using ProyectoFinalPOO2.Entities;

namespace ProyectoFinalPOO2.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly CafeteriaContext _context;

        public UsuariosController(CafeteriaContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Where(u => u.Activo)
                .Select(u => new
                {
                    u.Id,
                    u.Nombre,
                    u.Email,
                    u.FotoPerfil
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Id == id && u.Activo)
                .Select(u => new
                {
                    u.Id,
                    u.Nombre,
                    u.Email,
                    u.FotoPerfil,
                    TotalPedidos = u.Pedidos.Count
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            return Ok(usuario);
        }

        // POST: api/Usuarios/registro
        [HttpPost("registro")]
        public async Task<ActionResult<object>> Registro([FromBody] RegistroRequest request)
        {
            // Validar que el email no exista
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "El email ya está registrado" });
            }

            var usuario = new UsuarioEntity
            {
                Nombre = request.Nombre,
                Email = request.Email,
                Password = request.Password, // TODO: Hashear en producción
                FotoPerfil = request.FotoPerfil ?? $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(request.Nombre)}&background=831D81&color=fff",
                Activo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Email,
                usuario.FotoPerfil,
                message = "Usuario registrado exitosamente"
            });
        }

        // POST: api/Usuarios/login
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password && u.Activo);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Email o contraseña incorrectos" });
            }

            return Ok(new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Email,
                usuario.FotoPerfil,
                message = "Login exitoso"
            });
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] ActualizarUsuarioRequest request)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            usuario.Nombre = request.Nombre ?? usuario.Nombre;
            usuario.FotoPerfil = request.FotoPerfil ?? usuario.FotoPerfil;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario actualizado exitosamente" });
        }

        // Modelos de Request
        public class RegistroRequest
        {
            public string Nombre { get; set; } = "";
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
            public string? FotoPerfil { get; set; }
        }

        public class LoginRequest
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }

        public class ActualizarUsuarioRequest
        {
            public string? Nombre { get; set; }
            public string? FotoPerfil { get; set; }
        }
    }
}
