using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webTFGBack.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        public int id_cliente { get; set; }

        [ForeignKey("Persona")]
        public int id_persona { get; set; }

        // Navegación
        public Persona? Persona { get; set; }
        public ICollection<Suscripcion> Suscripciones { get; set; } = new List<Suscripcion>();
    }
}
