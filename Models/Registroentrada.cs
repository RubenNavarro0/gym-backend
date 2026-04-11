using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webTFGBack.Models
{
    [Table("Registro_Entrada")]
    public class RegistroEntrada
    {
        [Key]
        public int id_registro { get; set; }

        [Required]
        public DateTime fecha_hora_entrada { get; set; }

        public DateTime? fecha_hora_salida { get; set; }

        [ForeignKey("Persona")]
        public int id_persona { get; set; }

        [ForeignKey("Gym")]
        public int id_gym { get; set; }

        // Navegación
        public Persona? Persona { get; set; }
        public Gym? Gym { get; set; }
    }
}