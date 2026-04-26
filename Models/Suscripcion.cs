using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webTFGBack.Models
{
    [Table("Suscripcion")]
    public class Suscripcion
    {
        [Key]
        public int id_suscripcion { get; set; }

        [Required]
        public DateOnly fecha_inicio { get; set; }

        [Required]
        public DateOnly fecha_fin { get; set; }

        [Required]
        public int accesos_restantes { get; set; }

        [Required]
        [MaxLength(10)]
        public string estado { get; set; } = "activa"; // 'activa' | 'inactiva' | 'vencida'

        [ForeignKey("Cliente")]
        public int id_cliente { get; set; }

        [ForeignKey("Plan")]
        public int id_plan { get; set; }

        // Navegación
        public Cliente? Cliente { get; set; }
        public Plan? Plan { get; set; }
    }
}

