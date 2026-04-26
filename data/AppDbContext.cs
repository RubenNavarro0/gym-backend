using webTFGBack.Models;
using Microsoft.EntityFrameworkCore;

namespace webTFGBack.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Compania> Compania { get; set; }
        public DbSet<Gym> Gym { get; set; }
        public DbSet<Plan> Plan { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Trabajador> Trabajador { get; set; }
        public DbSet<Suscripcion> Suscripcion { get; set; }
        public DbSet<RegistroEntrada> RegistroEntrada { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Compania
            modelBuilder.Entity<Compania>().ToTable("Compania");
            modelBuilder.Entity<Compania>().HasKey(c => c.id_compania);

            // Gym
            modelBuilder.Entity<Gym>().ToTable("Gym");
            modelBuilder.Entity<Gym>().HasKey(g => g.id_gym);
            modelBuilder.Entity<Gym>()
                .HasOne(g => g.Compania)
                .WithMany(c => c.Gyms)
                .HasForeignKey(g => g.id_compania)
                .OnDelete(DeleteBehavior.Restrict);

            // Plan
            modelBuilder.Entity<Plan>().ToTable("Plan");
            modelBuilder.Entity<Plan>().HasKey(p => p.id_plan);
            modelBuilder.Entity<Plan>()
                .HasOne(p => p.Compania)
                .WithMany(c => c.Planes)
                .HasForeignKey(p => p.id_compania)
                .OnDelete(DeleteBehavior.Restrict);

            // Persona
            modelBuilder.Entity<Persona>().ToTable("Persona");
            modelBuilder.Entity<Persona>().HasKey(p => p.id_persona);
            modelBuilder.Entity<Persona>()
                .HasIndex(p => p.email).IsUnique();
            modelBuilder.Entity<Persona>()
                .HasIndex(p => p.documento_identidad).IsUnique();

            // Cliente (1:1 con Persona)
            modelBuilder.Entity<Cliente>().ToTable("Cliente");
            modelBuilder.Entity<Cliente>().HasKey(c => c.id_cliente);
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Persona)
                .WithOne(p => p.Cliente)
                .HasForeignKey<Cliente>(c => c.id_persona)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.id_persona).IsUnique();

            // Trabajador (1:1 con Persona)
            modelBuilder.Entity<Trabajador>().ToTable("Trabajador");
            modelBuilder.Entity<Trabajador>().HasKey(t => t.id_trabajador);
            modelBuilder.Entity<Trabajador>()
                .HasOne(t => t.Persona)
                .WithOne(p => p.Trabajador)
                .HasForeignKey<Trabajador>(t => t.id_persona)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Trabajador>()
                .HasIndex(t => t.id_persona).IsUnique();
            modelBuilder.Entity<Trabajador>()
                .HasOne(t => t.Gym)
                .WithMany(g => g.Trabajadores)
                .HasForeignKey(t => t.id_gym)
                .OnDelete(DeleteBehavior.Cascade);

            // Suscripcion
            modelBuilder.Entity<Suscripcion>().ToTable("Suscripcion");
            modelBuilder.Entity<Suscripcion>().HasKey(s => s.id_suscripcion);
            modelBuilder.Entity<Suscripcion>()
                .HasOne(s => s.Cliente)
                .WithMany(c => c.Suscripciones)
                .HasForeignKey(s => s.id_cliente)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Suscripcion>()
                .HasOne(s => s.Plan)
                .WithMany(p => p.Suscripciones)
                .HasForeignKey(s => s.id_plan)
                .OnDelete(DeleteBehavior.Restrict);

            // RegistroEntrada
            modelBuilder.Entity<RegistroEntrada>().ToTable("Registro_Entrada");
            modelBuilder.Entity<RegistroEntrada>().HasKey(r => r.id_registro);
            modelBuilder.Entity<RegistroEntrada>()
                .HasOne(r => r.Persona)
                .WithMany(p => p.RegistrosEntrada)
                .HasForeignKey(r => r.id_persona)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RegistroEntrada>()
                .HasOne(r => r.Gym)
                .WithMany(g => g.RegistrosEntrada)
                .HasForeignKey(r => r.id_gym)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}