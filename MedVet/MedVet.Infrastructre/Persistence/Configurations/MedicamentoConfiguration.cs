using MedVet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedVet.Infrastructure.Persistence.Configurations;

public class MedicamentoConfiguration : IEntityTypeConfiguration<Medicamento>
{
    public void Configure(EntityTypeBuilder<Medicamento> builder)
    {
        // Table name
        builder.ToTable("PJ_MEDICAMENTOS");

        // Primary key
        builder.HasKey(m => m.Id);

        // Id configuration
        builder.Property(m => m.Id)
            .HasColumnType("NUMBER(10)")
            .ValueGeneratedOnAdd();


        // Preco (Price)
        builder.Property(m => m.Preco)
            .HasColumnType("NUMBER(10,2)");
        

        builder.HasMany(m => m.Prescricoes)
            .WithMany(p => p.Medicamento)
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