using MedVet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedVet.Infrastructure.Persistence.Configurations;

public class DonoConfiguration: IEntityTypeConfiguration<Dono>
{
    public void Configure(EntityTypeBuilder<Dono> builder)
    {
        builder.ToTable("PJ_DONOS");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnType("NUMBER(10)")
            .ValueGeneratedOnAdd();

        builder.Property(d => d.Nome)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnType("VARCHAR2(150)");
        

        builder.Property(d => d.Telefone)
            .HasMaxLength(20)
            .HasColumnType("VARCHAR2(20)");

        builder.Property(d => d.Email)
            .HasMaxLength(100)
            .HasColumnType("VARCHAR2(100)");
        

        // Relacionamento um-para-muitos com Pet
        builder.HasMany(d => d.Pets)
            .WithOne(p => p.Dono)
            .HasForeignKey(p => p.IdDono)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        builder.HasIndex(d => d.Email)
            .HasDatabaseName("IX_DONOS_EMAIL");
        
        builder.HasIndex(d => d.Nome)
            .HasDatabaseName("IX_DONOS_NOME");
    }
}