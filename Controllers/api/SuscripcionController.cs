using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webTFGBack.data;

namespace webTFGBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuscripcionController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SuscripcionController(AppDbContext context) => _context = context;

        // GET api/suscripcion/resumen
        [HttpGet("resumen")]
        public async Task<IActionResult> GetResumen()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var inicioMes = new DateOnly(hoy.Year, hoy.Month, 1);

            var suscMes = await _context.Suscripcion
                .Include(s => s.Plan)
                .Where(s => s.fecha_inicio >= inicioMes)
                .ToListAsync();

            var facturado = suscMes.Sum(s => s.Plan?.precio ?? 0);
            var cobrado = suscMes.Where(s => s.estado == "activa").Sum(s => s.Plan?.precio ?? 0);
            var pendiente = facturado - cobrado;
            var vencidas = await _context.Suscripcion
                .CountAsync(s => s.estado == "vencida" && s.fecha_fin >= inicioMes);

            return Ok(new { facturado, cobrado, pendiente, vencidas });
        }

        // GET api/suscripcion/ingresos-mensuales
        // Devuelve los últimos 6 meses agrupados
        [HttpGet("ingresos-mensuales")]
        public async Task<IActionResult> GetIngresosMensuales()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var desde = hoy.AddMonths(-5).AddDays(1 - hoy.Day); // inicio del mes de hace 5 meses

            var datos = await _context.Suscripcion
                .Include(s => s.Plan)
                .Where(s => s.fecha_inicio >= desde)
                .GroupBy(s => new { s.fecha_inicio.Year, s.fecha_inicio.Month })
                .Select(g => new
                {
                    year = g.Key.Year,
                    month = g.Key.Month,
                    total = g.Sum(s => s.Plan!.precio)
                })
                .OrderBy(x => x.year).ThenBy(x => x.month)
                .ToListAsync();

            // Normalizar a porcentaje relativo al máximo
            double maxVal = datos.Any() ? (double)datos.Max(d => d.total) : 1;
            var meses = new[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

            var resultado = datos.Select(d => new
            {
                month = meses[d.month - 1],
                total = d.total,
                pct = maxVal > 0 ? Math.Round((double)d.total / maxVal * 100, 1) : 0,
                active = d.year == hoy.Year && d.month == hoy.Month
            });

            return Ok(resultado);
        }

        // GET api/suscripcion/recientes
        [HttpGet("recientes")]
        public async Task<IActionResult> GetRecientes()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            var cuotas = await _context.Suscripcion
                .Include(s => s.Cliente).ThenInclude(c => c!.Persona)
                .Include(s => s.Plan)
                .OrderByDescending(s => s.fecha_inicio)
                .Take(10)
                .Select(s => new
                {
                    id = s.id_suscripcion,
                    nombre = s.Cliente!.Persona!.nombre,
                    plan = s.Plan!.nombre,
                    monto = s.Plan!.precio,
                    vence = s.fecha_fin.ToString("dd MMM"),
                    estado = s.estado   // 'activa' | 'inactiva' | 'vencida'
                })
                .ToListAsync();

            return Ok(cuotas);
        }
    }
}
