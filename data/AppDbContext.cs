using webTFGBack.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace webTFGBack.data 
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Gym> Gym { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Membresia> Membresias { get; set; }
        public DbSet<Trabajador_GYM> Trabajador_GYM { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().ToTable("Cliente");
            modelBuilder.Entity<Cliente>().HasKey(c => c.id_cliente); // <-- define primary key

            modelBuilder.Entity<Gym>().ToTable("Gym");
            modelBuilder.Entity<Gym>().HasKey(g => g.id_gym);

            modelBuilder.Entity<Membresia>().ToTable("Membresias");
            modelBuilder.Entity<Membresia>().HasKey(m => m.id_membresia);

            modelBuilder.Entity<Trabajador_GYM>().ToTable("Trabajador_GYM");
            modelBuilder.Entity<Trabajador_GYM>().HasKey(t => t.id_trabajadorGYM);
        }
    }
}
