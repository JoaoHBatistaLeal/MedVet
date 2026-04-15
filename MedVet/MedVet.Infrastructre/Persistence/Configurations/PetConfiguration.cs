using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MedVet.Domain.Entities;

namespace MedVet.Infrastructure.Persistence.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("PJ_ANIMAIS");
     
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnType("RAW(16)")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("VARCHAR2(100)");

        builder.Property(p => p.TipoAnimal)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("VARCHAR2(50)")
            .HasColumnName("TIPO_ANIMAL");
        
        builder.Property(p => p.Raca)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("VARCHAR2(50)");
       
        builder.Property(p => p.Genero)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnType("VARCHAR2(20)");

        builder.Property(p => p.IdDono)
            .IsRequired()
            .HasColumnType("RAW(16)")
            .HasColumnName("ID_DONO");
        
        builder.HasMany(p => p.Consultas)
            .WithOne(c => c.Pet)
            .HasForeignKey(c => c.IdPet)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.IdDono).HasDatabaseName("IX_ANIMAIS_ID_DONO");
        builder.HasIndex(p => p.Nome).HasDatabaseName("IX_ANIMAIS_NOME");
        builder.HasIndex(p => p.TipoAnimal).HasDatabaseName("IX_ANIMAIS_TIPO_ANIMAL");
        builder.HasIndex(p => p.Raca).HasDatabaseName("IX_ANIMAIS_RACA");
    }
}