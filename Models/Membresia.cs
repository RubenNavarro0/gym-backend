namespace webTFGBack.Models
{
    public class Membresia
    {
        public int id_membresia { get; set; }

        public int id_gym { get; set; }
        public int id_cliente { get; set; }

        public string tipo_membresia { get; set; }

        public bool isActive { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_fin { get; set; }

        public Gym Gym { get; set; }
        public Cliente Cliente { get; set; }
    }
}
