using MedVet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedVet.Infrastructure.Persistence
{
    public class MedVetContext : DbContext
    {
        public MedVetContext(DbContextOptions<MedVetContext> options) : base(options)
        {
        }

        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Dono> Donos { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Prescricao> Prescricoes { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MedVetContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        }
    
    
}