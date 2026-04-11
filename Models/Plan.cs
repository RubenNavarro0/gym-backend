using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webTFGBack.Models
{
    [Table("Plan")]
    public class Plan
    {
        [Key]
        public int id_plan { get; set; }

        [Required]
        [MaxLength(100)]
        public string nombre { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal precio { get; set; }

        [Required]
        public int duracion_dias { get; set; }

        [Required]
        public int total_accesos { get; set; }

        [Required]
        [MaxLength(50)]
        public string tipo { get; set; } = string.Empty;

        [ForeignKey("Compania")]
        public int id_compania { get; set; }

        // Navegación
        public Compania? Compania { get; set; }
        public ICollection<Suscripcion> Suscripciones { get; set; } = new List<Suscripcion>();
    }
}
