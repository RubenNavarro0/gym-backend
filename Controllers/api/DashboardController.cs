using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webTFGBack.data;

namespace webTFGBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context) => _context = context;

        // GET api/dashboard/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var inicioMes = new DateOnly(hoy.Year, hoy.Month, 1);

            var totalClientes = await _context.Cliente.CountAsync();

            // Suscripciones activas con precio (ingresos del mes = suscripciones iniciadas este mes)
            var ingresosMes = await _context.Suscripcion
                .Include(s => s.Plan)
                .Where(s => s.fecha_inicio >= inicioMes && s.fecha_inicio <= hoy)
                .SumAsync(s => (decimal?)s.Plan!.precio) ?? 0;

            var vencenHoy = await _context.Suscripcion
                .CountAsync(s => s.fecha_fin == hoy && s.estado == "activa");

            var altasMes = await _context.Suscripcion
                .CountAsync(s => s.fecha_inicio >= inicioMes && s.fecha_inicio <= hoy);

            return Ok(new
            {
                totalClientes,
                ingresosMes,
                vencenHoy,
                altasMes
            });
        }

        // GET api/dashboard/miembros-recientes
        [HttpGet("miembros-recientes")]
        public async Task<IActionResult> GetMiembrosRecientes()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var en7Dias = hoy.AddDays(7);

            var miembros = await _context.Cliente
                .Include(c => c.Persona)
                .Include(c => c.Suscripciones)
                    .ThenInclude(s => s.Plan)
                .OrderByDescending(c => c.id_cliente)
                .Take(8)
                .Select(c => new
                {
                    id = c.id_cliente,
                    nombre = c.Persona!.nombre,
                    plan = c.Suscripciones
                                 .Where(s => s.estado == "activa")
                                 .OrderByDescending(s => s.fecha_inicio)
                                 .Select(s => s.Plan!.nombre)
                                 .FirstOrDefault() ?? "Sin plan",
                    // "warning" si vence en <=7 días, "active" si no
                    status = c.Suscripciones
                                 .Any(s => s.estado == "activa" && s.fecha_fin <= en7Dias)
                                 ? "warning" : "active"
                })
                .ToListAsync();

            return Ok(miembros);
        }

        // GET api/dashboard/ocupacion
        [HttpGet("ocupacion")]
        public async Task<IActionResult> GetOcupacion()
        {
            // Personas con entrada hoy sin salida registrada = dentro del gym ahora mismo
            var hoyDt = DateTime.Today;
            var manyanaDt = hoyDt.AddDays(1);

            var dentro = await _context.RegistroEntrada
                .CountAsync(r =>
                    r.fecha_hora_entrada >= hoyDt &&
                    r.fecha_hora_entrada < manyanaDt &&
                    r.fecha_hora_salida == null);

            // Total de entradas hoy (para calcular % respecto al aforo)
            var totalHoy = await _context.RegistroEntrada
                .CountAsync(r =>
                    r.fecha_hora_entrada >= hoyDt &&
                    r.fecha_hora_entrada < manyanaDt);

            // Aforo máximo configurable; si no hay datos usamos 100 para que el % tenga sentido
            int aforoMax = 100;

            return Ok(new
            {
                dentro,
                totalHoy,
                aforoMax,
                porcentaje = aforoMax > 0 ? Math.Round((double)dentro / aforoMax * 100, 1) : 0
            });
        }
    }
}