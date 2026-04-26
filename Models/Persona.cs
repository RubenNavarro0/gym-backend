using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webTFGBack.Models
{
    [Table("Persona")]
    public class Persona
    {
        [Key]
        public int id_persona { get; set; }

        [Required]
        [MaxLength(150)]
        public string nombre { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? email { get; set; }

        [Required]
        [MaxLength(50)]
        public string documento_identidad { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? telefono { get; set; }

        [Required]
        [MaxLength(20)]
        public string pass { get; set; } = string.Empty;

        // Navegación
        public Cliente? Cliente { get; set; }
        public Trabajador? Trabajador { get; set; }
        public ICollection<RegistroEntrada> RegistrosEntrada { get; set; } = new List<RegistroEntrada>();
    }
}