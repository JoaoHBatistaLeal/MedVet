using MedVet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace MedVet.Infrastructure.Persistence.Configurations;

public class PrescricaoConfiguration : IEntityTypeConfiguration<Prescricao> 
{
    public void Configure(EntityTypeBuilder<Prescricao> builder) 
    {
        builder.ToTable("PJ_PRESCRICOES");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasColumnType("RAW(16)")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.IdConsulta)
            .IsRequired()
            .HasColumnType("RAW(16)")
            .HasColumnName("ID_CONSULTA");

        builder.HasMany(p => p.Medicamentos) 
            .WithMany(m => m.Prescricoes)
            .UsingEntity<Dictionary<string, object>>(
                "PJ_PRESCRICOES_MEDICAMENTOS",
                j => j.HasOne<Medicamento>().WithMany().HasForeignKey("IdMedicamento"),
                j => j.HasOne<Prescricao>().WithMany().HasForeignKey("IdPrescricao")
            );
        
        builder.HasIndex(p => p.IdConsulta)
            .IsUnique()
            .HasDatabaseName("IX_PRESCRICOES_ID_CONSULTA_UNIQUE");
    }
}