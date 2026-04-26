using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webTFGBack.data;

namespace webTFGBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GymController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GymController(AppDbContext context) => _context = context;

        // GET api/gym/info
        // Devuelve el primer gym disponible (en el futuro vendrá del JWT del trabajador)
        [HttpGet("info")]
        public async Task<IActionResult> GetInfo()
        {
            var gym = await _context.Gym
                .Include(g => g.Compania)
                .Select(g => new
                {
                    id_gym = g.id_gym,
                    nombre = g.nombre,
                    ciudad = g.ciudad,
                    compania = g.Compania!.nombre,
                    // Personas dentro ahora mismo (entrada hoy, sin salida)
                    activosHoy = _context.RegistroEntrada.Count(r =>
                        r.id_gym == g.id_gym &&
                        r.fecha_hora_entrada >= DateTime.Today &&
                        r.fecha_hora_salida == null)
                })
                .FirstOrDefaultAsync();

            if (gym == null)
                return NotFound(new { message = "No hay ningún gimnasio configurado" });

            return Ok(gym);
        }
    }
}
