using MedVet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedVet.Infrastructure.Persistence.Configurations;

public class MedicamentoConfiguration : IEntityTypeConfiguration<Medicamento>
{
    public void Configure(EntityTypeBuilder<Medicamento> builder)
    {
        builder.ToTable("PJ_MEDICAMENTOS");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnType("RAW(16)")
            .ValueGeneratedOnAdd();
        
        builder.Property(m => m.Preco)
            .HasColumnType("NUMBER(10,2)");
        

        builder.HasMany(m => m.Prescricoes)
            .WithMany(p => p.Medicamentos)
            .UsingEntity<Dictionary<string, object>>(
                "PJ_PRESCRICOES_MEDICAMENTOS",
                right => right.HasOne<Prescricao>()
                    .WithMany()
                    .HasForeignKey("PrescricaoId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left.HasOne<Medicamento>()
                    .WithMany()
                    .HasForeignKey("MedicamentoId")
                    .OnDelete(DeleteBehavior.Restrict),
                join =>
                {
                    join.ToTable("PJ_PRESCRICOES_MEDICAMENTOS");
                    join.HasKey("PrescricaoId", "MedicamentoId");

                    join.Property<DateTime>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("TIMESTAMP");

                    join.Property<bool>("Active")
                        .IsRequired()
                        .HasColumnType("NUMBER(1)");
                });


    }
    
}