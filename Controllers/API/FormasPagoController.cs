using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalPOO2.Data;

namespace ProyectoFinalPOO2.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormasPagoController : ControllerBase
    {
        private readonly CafeteriaContext _context;

        public FormasPagoController(CafeteriaContext context)
        {
            _context = context;
        }

        // GET: api/FormasPago
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetFormasPago()
        {
            var formasPago = await _context.FormasDePago
                .Where(f => f.Activo)
                .Select(f => new
                {
                    f.Id,
                    f.Nombre,
                    f.Descripcion
                })
                .ToListAsync();

            return Ok(formasPago);
        }
    }
}
