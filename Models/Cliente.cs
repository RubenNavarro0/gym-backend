using System.ComponentModel.DataAnnotations;
using webTFGBack.Models;

namespace webTFGBack.Models
{
    public class Cliente
    {
        public int id_cliente { get; set; }
        public required string correo { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public required string pass { get; set; }
        public string dni { get; set; }

        public List<Membresia>? Membresias { get; set; }
    }
}
