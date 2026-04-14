using MedVet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedVet.Infrastructure.Persistence.Configurations;

public class VeterinarioConfiguration : IEntityTypeConfiguration<Veterinario>
{
    public void Configure(EntityTypeBuilder<Veterinario> builder)
    {
        // Table name
        builder.ToTable("PJ_VETERINARIOS");

        // Primary key
        builder.HasKey(v => v.Id);

        // Id configuration
        builder.Property(v => v.Id)
            .HasColumnType("NUMBER(10)")
            .ValueGeneratedOnAdd();

        // Nome (Name)
        builder.Property(v => v.Nome)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("VARCHAR2(200)");

        // Crmv (Conselho Regional de Medicina Veterinária)
        builder.Property(v => v.Crmv)
            .IsRequired()
            .HasColumnType("NUMBER(10)")
            .HasColumnName("CRMV");

        // Especialidade (Specialty)
        builder.Property(v => v.Especialidade)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("VARCHAR2(100)");
        

        // Relationship with Consulta (one-to-many)
        builder.HasMany(v => v.Consultas)
            .WithOne(c => c.Veterinario)
            .HasForeignKey(c => c.IdVeterinario)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deleting veterinarian with existing consultations

        // Indexes for better performance
        builder.HasIndex(v => v.Nome)
            .HasDatabaseName("IX_VETERINARIOS_NOME");

        builder.HasIndex(v => v.Crmv)
            .IsUnique() // CRMV should be unique
            .HasDatabaseName("IX_VETERINARIOS_CRMV_UNIQUE");

        builder.HasIndex(v => v.Especialidade)
            .HasDatabaseName("IX_VETERINARIOS_ESPECIALIDADE");

        // Composite index for common searches
        builder.HasIndex(v => new { v.Nome, v.Especialidade })
            .HasDatabaseName("IX_VETERINARIOS_NOME_ESPECIALIDADE");
    }
}