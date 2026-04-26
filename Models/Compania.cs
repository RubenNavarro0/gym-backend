using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace webTFGBack.Models
{
    [Table("Compania")]
    public class Compania
    {
        [Key]
        public int id_compania { get; set; }

        [Required]
        [MaxLength(100)]
        public string nombre { get; set; } = string.Empty;

        // Navegación
        public ICollection<Gym> Gyms { get; set; } = new List<Gym>();
        public ICollection<Plan> Planes { get; set; } = new List<Plan>();
    }
}
