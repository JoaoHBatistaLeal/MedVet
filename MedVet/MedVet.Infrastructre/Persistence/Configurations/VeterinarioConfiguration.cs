using MedVet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedVet.Infrastructure.Persistence.Configurations;

public class VeterinarioConfiguration : IEntityTypeConfiguration<Veterinario>
{
    public void Configure(EntityTypeBuilder<Veterinario> builder)
    {
        builder.ToTable("PJ_VETERINARIOS");
        
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.Id)
            .HasColumnType("NUMBER(10)")
            .ValueGeneratedOnAdd();
        
        builder.Property(v => v.Nome)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("VARCHAR2(200)");
        
        builder.Property(v => v.Crmv)
            .IsRequired()
            .HasColumnType("NUMBER(10)")
            .HasColumnName("CRMV");

        builder.Property(v => v.Especialidade)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("VARCHAR2(100)");

        builder.HasMany(v => v.Consultas)
            .WithOne(c => c.Veterinario)
            .HasForeignKey(c => c.IdVeterinario)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.HasIndex(v => v.Nome)
            .HasDatabaseName("IX_VETERINARIOS_NOME");

        builder.HasIndex(v => v.Crmv)
            .IsUnique() 
            .HasDatabaseName("IX_VETERINARIOS_CRMV_UNIQUE");

        builder.HasIndex(v => v.Especialidade)
            .HasDatabaseName("IX_VETERINARIOS_ESPECIALIDADE");

        builder.HasIndex(v => new { v.Nome, v.Especialidade })
            .HasDatabaseName("IX_VETERINARIOS_NOME_ESPECIALIDADE");
    }
}