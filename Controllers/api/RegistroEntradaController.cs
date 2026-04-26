using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webTFGBack.data;
using webTFGBack.Models;

namespace webTFGBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroEntradaController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RegistroEntradaController(AppDbContext context) => _context = context;

        // POST api/registroentrada/checkin
        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInRequest req)
        {
            // Buscar persona por nombre o documento
            var persona = await _context.Persona
                .FirstOrDefaultAsync(p =>
                    p.nombre.Contains(req.busqueda) ||
                    p.documento_identidad == req.busqueda);

            if (persona == null)
                return NotFound(new { message = "Persona no encontrada" });

            // Verificar que tenga suscripción activa
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var cliente = await _context.Cliente
                .Include(c => c.Suscripciones)
                .FirstOrDefaultAsync(c => c.id_persona == persona.id_persona);

            if (cliente == null)
                return BadRequest(new { message = "La persona no es un cliente registrado" });

            bool tieneAcceso = cliente.Suscripciones
                .Any(s => s.estado == "activa" && s.fecha_fin >= hoy && s.accesos_restantes > 0);

            if (!tieneAcceso)
                return BadRequest(new { message = "El cliente no tiene suscripción activa o sin accesos" });

            // Buscar el gym del trabajador logueado (por ahora tomamos el primer gym disponible)
            // En el futuro esto vendrá del token JWT
            var gym = await _context.Gym.FirstOrDefaultAsync();
            if (gym == null)
                return BadRequest(new { message = "No hay ningún gimnasio configurado" });

            var registro = new RegistroEntrada
            {
                id_persona = persona.id_persona,
                id_gym = gym.id_gym,
                fecha_hora_entrada = DateTime.Now,
                fecha_hora_salida = null
            };

            _context.RegistroEntrada.Add(registro);

            // Descontar un acceso de la suscripción activa más próxima a vencer
            var suscripcion = cliente.Suscripciones
                .Where(s => s.estado == "activa" && s.fecha_fin >= hoy && s.accesos_restantes > 0)
                .OrderBy(s => s.fecha_fin)
                .First();

            suscripcion.accesos_restantes--;
            if (suscripcion.accesos_restantes == 0)
                suscripcion.estado = "inactiva";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Check-in registrado para {persona.nombre}",
                id_registro = registro.id_registro,
                accesos_restantes = suscripcion.accesos_restantes
            });
        }
    }

    public class CheckInRequest
    {
        public string busqueda { get; set; } = string.Empty; // nombre o DNI
    }
}
