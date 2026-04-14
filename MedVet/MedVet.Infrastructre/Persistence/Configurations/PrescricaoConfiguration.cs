// using MedVet.Domain.Entities;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace MedVet.Infrastructure.Persistence.Configurations;
//
// public class PrescricaoConfiguration : IEntityTypeConfiguration<Prescricao> 
// {
//     public void Configure(EntityTypeBuilder<Prescricao> builder) 
//     {
//         builder.ToTable("PJ_PRESCRICOES");
//         builder.HasKey(p => p.Id);
//
//         builder.Property(p => p.Id)
//             .HasColumnType("NUMBER(10)")
//             .ValueGeneratedOnAdd();
//
//         builder.Property(p => p.IdConsulta)
//             .IsRequired()
//             .HasColumnType("RAW(16)")
//             .HasColumnName("ID_CONSULTA");
//         
//         builder.HasMany(p => p.Medicamento)
//             .WithMany(m => m.Prescricoes)
//             .UsingEntity<Dictionary<string, object>>(
//                 "PJ_PRESCRICOES_MEDICAMENTOS",
//                 right => right.HasOne<MedicamentoConfiguration>()
//                     .WithMany()
//                     .HasForeignKey("MedicamentoId")
//                     .OnDelete(DeleteBehavior.Restrict),
//                 left => left.HasOne<Prescricao>()
//                     .WithMany()
//                     .HasForeignKey("PrescricaoId")
//                     .OnDelete(DeleteBehavior.Cascade),
//                 join =>
//                 {
//                     join.ToTable("PJ_PRESCRICOES_MEDICAMENTOS");
//                     join.HasKey("PrescricaoId", "MedicamentoId");
//
//                     join.Property<DateTime>("CreatedAt")
//                         .IsRequired()
//                         .HasColumnType("TIMESTAMP");
//
//                     join.Property<bool>("Active")
//                         .IsRequired()
//                         .HasColumnType("NUMBER(1)");
//                 });
//         
//         builder.HasIndex(p => p.IdConsulta)
//             .IsUnique()
//             .HasDatabaseName("IX_PRESCRICOES_ID_CONSULTA_UNIQUE");
//     }
// }
using MedVet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedVet.Infrastructure.Persistence.Configurations;

// Primeiro, crie uma classe para a tabela de junção
public class PrescricaoMedicamento
{
    public long PrescricaoId { get; set; }
    public long MedicamentoId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Active { get; set; }
    
    public virtual Prescricao Prescricao { get; set; }
    public virtual Medicamento Medicamento { get; set; }
}

public class PrescricaoConfiguration : IEntityTypeConfiguration<Prescricao> 
{
    public void Configure(EntityTypeBuilder<Prescricao> builder) 
    {
        builder.ToTable("PJ_PRESCRICOES");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnType("NUMBER(10)")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.IdConsulta)
            .IsRequired()
            .HasColumnType("RAW(16)")
            .HasColumnName("ID_CONSULTA");
        
        // Configuração muitos-para-muitos com entidade de junção
        builder.HasMany(p => p.Medicamento)
            .WithMany(m => m.Prescricoes)
            .UsingEntity<PrescricaoMedicamento>(
                j => j.HasOne(pm => pm.Medicamento)
                    .WithMany()
                    .HasForeignKey(pm => pm.MedicamentoId)
                    .OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne(pm => pm.Prescricao)
                    .WithMany()
                    .HasForeignKey(pm => pm.PrescricaoId)
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.ToTable("PJ_PRESCRICOES_MEDICAMENTOS");
                    j.HasKey(pm => new { pm.PrescricaoId, pm.MedicamentoId });
                    j.Property(pm => pm.CreatedAt).IsRequired().HasColumnType("TIMESTAMP");
                    // j.Property(pm => pm.Active).IsRequired().HasColumnType("NUMBER(1));
                });
        
        builder.HasIndex(p => p.IdConsulta)
            .IsUnique()
            .HasDatabaseName("IX_PRESCRICOES_ID_CONSULTA_UNIQUE");
    }
}