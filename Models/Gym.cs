using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webTFGBack.Models
{
    [Table("Gym")]
    public class Gym
    {
        [Key]
        public int id_gym { get; set; }

        [Required]
        [MaxLength(100)]
        public string nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ciudad { get; set; } = string.Empty;

        [ForeignKey("Compania")]
        public int id_compania { get; set; }

        // Navegación
        public Compania? Compania { get; set; }
        public ICollection<Trabajador> Trabajadores { get; set; } = new List<Trabajador>();
        public ICollection<RegistroEntrada> RegistrosEntrada { get; set; } = new List<RegistroEntrada>();
    }
}
