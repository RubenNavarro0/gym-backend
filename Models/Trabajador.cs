using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webTFGBack.Models
{
    [Table("Trabajador")]
    public class Trabajador
    {
        [Key]
        public int id_trabajador { get; set; }

        [ForeignKey("Persona")]
        public int id_persona { get; set; }

        [Required]
        [MaxLength(80)]
        public string rol { get; set; } = string.Empty;

        [ForeignKey("Gym")]
        public int id_gym { get; set; }

        // Navegación
        public Persona? Persona { get; set; }
        public Gym? Gym { get; set; }
    }
}