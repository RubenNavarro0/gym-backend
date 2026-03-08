namespace webTFGBack.Models
{
    public class Trabajador_GYM
    {
        public int id_trabajadorGYM { get; set; }

        public int id_gym { get; set; }

        public string rol { get; set; }

        public string usuario { get; set; }

        public string nombre { get; set; }

        public string dni { get; set; }

        public Gym Gym { get; set; }
    }
}
