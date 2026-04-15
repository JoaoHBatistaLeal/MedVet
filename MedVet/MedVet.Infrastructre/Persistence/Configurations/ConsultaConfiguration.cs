using MedVet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedVet.Infrastructure.Persistence.Configurations;

public class ConsultaConfiguration : IEntityTypeConfiguration<Consulta>
{
    public void Configure(EntityTypeBuilder<Consulta> builder)
    {
        builder.ToTable("PJ_CONSULTAS");

        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Id)
            .HasColumnType("RAW(16)")
            .ValueGeneratedOnAdd();
        
        builder.Property(c => c.IdPet)
            .IsRequired()
            .HasColumnType("RAW(16)")
            .HasColumnName("ID_PET");
        
        builder.Property(c => c.IdVeterinario)
            .IsRequired()
            .HasColumnType("RAW(16)")
            .HasColumnName("ID_VETERINARIO");
        
        builder.Property(c => c.DataConsulta)
            .IsRequired()
            .HasColumnType("TIMESTAMP(7)")
            .HasColumnName("DATA_CONSULTA");
        
        builder.Property(c => c.Diagnostico)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnType("VARCHAR2(500)");

        builder.Property(c => c.Observacoes)
            .HasMaxLength(1000)
            .HasColumnType("VARCHAR2(1000)");

        builder.HasOne(c => c.Pet)
            .WithMany(p => p.Consultas)
            .HasForeignKey(c => c.IdPet)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.IdPet)
            .HasDatabaseName("IX_CONSULTAS_ID_PET");

        builder.HasIndex(c => c.IdVeterinario)
            .HasDatabaseName("IX_CONSULTAS_ID_VETERINARIO");

        builder.HasIndex(c => c.DataConsulta)
            .HasDatabaseName("IX_CONSULTAS_DATA_CONSULTA");

        builder.HasIndex(c => new { c.IdPet, c.DataConsulta })
            .HasDatabaseName("IX_CONSULTAS_PET_DATA");
    }

}