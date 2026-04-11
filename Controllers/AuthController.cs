using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webTFGBack.data;

namespace webTFGBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.email) || string.IsNullOrWhiteSpace(request.pass))
                return BadRequest(new { message = "Email y contraseña son obligatorios" });

            var emailNorm = request.email.Trim().ToLower();

            // 1 — Buscar Persona por email
            var persona = await _context.Persona
                .FirstOrDefaultAsync(p => p.email != null &&
                                          p.email.ToLower().Trim() == emailNorm);

            if (persona == null)
                return Unauthorized(new { message = "Email o contraseña incorrectos" });

            // 2 — Verificar contraseña
            if (persona.pass.Trim() != request.pass.Trim())
                return Unauthorized(new { message = "Email o contraseña incorrectos" });

            // 3 — Verificar que es Trabajador
            var trabajador = await _context.Trabajador
                .FirstOrDefaultAsync(t => t.id_persona == persona.id_persona);

            if (trabajador == null)
                return Unauthorized(new { message = "Acceso denegado: solo trabajadores pueden entrar" });

            return Ok(new
            {
                message = "Login correcto",
                id_trabajador = trabajador.id_trabajador,
                rol = trabajador.rol,
                nombre = persona.nombre,
                email = persona.email
            });
        }
    }

    public class LoginRequest
    {
        public string email { get; set; } = string.Empty;
        public string pass { get; set; } = string.Empty;
    }
}