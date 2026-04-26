using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webTFGBack.data;

namespace webTFGBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PlanController(AppDbContext context) => _context = context;

        // GET api/plan
        [HttpGet]
        public async Task<IActionResult> GetPlanes()
        {
            var planes = await _context.Plan
                .Select(p => new
                {
                    id = p.id_plan,
                    nombre = p.nombre,
                    precio = p.precio,
                    tipo = p.tipo,
                    miembros = _context.Suscripcion
                                .Count(s => s.id_plan == p.id_plan && s.estado == "activa")
                })
                .ToListAsync();

            // Calcular porcentaje relativo al plan con más socios
            int maxMiembros = planes.Any() ? planes.Max(p => p.miembros) : 1;

            var resultado = planes.Select(p => new
            {
                p.id,
                p.nombre,
                p.precio,
                p.tipo,
                p.miembros,
                pct = maxMiembros > 0
                    ? Math.Round((double)p.miembros / maxMiembros * 100, 1)
                    : 0
            });

            return Ok(resultado);
        }
    }
}