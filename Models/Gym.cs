namespace webTFGBack.Models
{
    public class Gym
    {
        public int id_gym { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string direccion { get; set; }
        public string pass { get; set; }

        public List<Membresia>? Membresias { get; set; }
        public List<Trabajador_GYM> Trabajadores { get; set; }
    }
}
