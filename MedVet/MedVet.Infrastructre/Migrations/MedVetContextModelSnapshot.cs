using MedVet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MedVet.Infrastructure.Migrations;


    [DbContext(typeof(MedVetContext))]
    partial class MedVetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            // Tabela Dono (Cliente)
            modelBuilder.Entity("MedVet.Domain.Entities.Dono", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("NUMBER(10)")
                    .HasColumnName("ID");

                OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<bool>("Active")
                    .HasColumnType("NUMBER(1)")
                    .HasColumnName("ACTIVE");

                b.Property<string>("Cpf")
                    .IsRequired()
                    .HasMaxLength(14)
                    .HasColumnType("VARCHAR2(14)")
                    .HasColumnName("CPF");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("TIMESTAMP")
                    .HasColumnName("CREATED_AT");

                b.Property<string>("Email")
                    .HasMaxLength(100)
                    .HasColumnType("VARCHAR2(100)")
                    .HasColumnName("EMAIL");

                b.Property<string>("Endereco")
                    .HasMaxLength(200)
                    .HasColumnType("VARCHAR2(200)")
                    .HasColumnName("ENDERECO");

                b.Property<string>("Nome")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("VARCHAR2(150)")
                    .HasColumnName("NOME");

                b.Property<string>("Telefone")
                    .HasMaxLength(20)
                    .HasColumnType("VARCHAR2(20)")
                    .HasColumnName("TELEFONE");

                b.HasKey("Id");

                b.HasIndex("Cpf")
                    .IsUnique()
                    .HasDatabaseName("IX_DONOS_CPF_UNIQUE");

                b.HasIndex("Email")
                    .HasDatabaseName("IX_DONOS_EMAIL");

                b.HasIndex("Nome")
                    .HasDatabaseName("IX_DONOS_NOME");

                b.ToTable("PJ_DONOS", (string)null);
            });

            // Tabela Pet (Animal)
            modelBuilder.Entity("MedVet.Domain.Entities.Pet", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("NUMBER(10)")
                    .HasColumnName("ID");

                OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<bool>("Active")
                    .HasColumnType("NUMBER(1)")
                    .HasColumnName("ACTIVE");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("TIMESTAMP")
                    .HasColumnName("CREATED_AT");

                b.Property<int?>("DonoId")
                    .HasColumnType("NUMBER(10)");

                b.Property<string>("Genero")
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("VARCHAR2(20)")
                    .HasColumnName("GENERO");

                b.Property<Guid>("IdDono")
                    .HasColumnType("RAW(16)")
                    .HasColumnName("ID_DONO");

                b.Property<string>("Nome")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("VARCHAR2(100)")
                    .HasColumnName("NOME");

                b.Property<string>("Raca")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("VARCHAR2(50)")
                    .HasColumnName("RACA");

                b.Property<string>("TipoAnimal")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("VARCHAR2(50)")
                    .HasColumnName("TIPO_ANIMAL");

                b.HasKey("Id");

                b.HasIndex("DonoId");

                b.HasIndex("IdDono")
                    .HasDatabaseName("IX_ANIMAIS_ID_DONO");

                b.HasIndex("Nome")
                    .HasDatabaseName("IX_ANIMAIS_NOME");

                b.HasIndex("TipoAnimal")
                    .HasDatabaseName("IX_ANIMAIS_TIPO_ANIMAL");

                b.HasIndex("Raca")
                    .HasDatabaseName("IX_ANIMAIS_RACA");

                b.ToTable("PJ_ANIMAIS", (string)null);
            });

            // Tabela Consulta
            modelBuilder.Entity("MedVet.Domain.Entities.Consulta", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("NUMBER(10)")
                    .HasColumnName("ID");

                OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<bool>("Active")
                    .HasColumnType("NUMBER(1)")
                    .HasColumnName("ACTIVE");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("TIMESTAMP")
                    .HasColumnName("CREATED_AT");

                b.Property<DateTime>("DataConsulta")
                    .HasColumnType("TIMESTAMP")
                    .HasColumnName("DATA_CONSULTA");

                b.Property<string>("Diagnostico")
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnType("VARCHAR2(500)")
                    .HasColumnName("DIAGNOSTICO");

                b.Property<Guid>("IdPet")
                    .HasColumnType("RAW(16)")
                    .HasColumnName("ID_PET");

                b.Property<Guid>("IdVeterinario")
                    .HasColumnType("RAW(16)")
                    .HasColumnName("ID_VETERINARIO");

                b.Property<string>("Observacoes")
                    .HasMaxLength(1000)
                    .HasColumnType("VARCHAR2(1000)")
                    .HasColumnName("OBSERVACOES");

                b.Property<int?>("PetId")
                    .HasColumnType("NUMBER(10)");

                b.HasKey("Id");

                b.HasIndex("DataConsulta")
                    .HasDatabaseName("IX_CONSULTAS_DATA_CONSULTA");

                b.HasIndex("IdPet")
                    .HasDatabaseName("IX_CONSULTAS_ID_PET");

                b.HasIndex("IdVeterinario")
                    .HasDatabaseName("IX_CONSULTAS_ID_VETERINARIO");

                b.HasIndex("PetId");

                b.HasIndex("IdPet", "DataConsulta")
                    .HasDatabaseName("IX_CONSULTAS_PET_DATA");

                b.ToTable("PJ_CONSULTAS", (string)null);
            });

            // Tabela Prescricao
            modelBuilder.Entity("MedVet.Domain.Entities.Prescricao", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("NUMBER(10)")
                    .HasColumnName("ID");

                OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<bool>("Active")
                    .HasColumnType("NUMBER(1)")
                    .HasColumnName("ACTIVE");

                b.Property<int?>("ConsultaId")
                    .HasColumnType("NUMBER(10)");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("TIMESTAMP")
                    .HasColumnName("CREATED_AT");

                b.Property<Guid>("IdConsulta")
                    .HasColumnType("RAW(16)")
                    .HasColumnName("ID_CONSULTA");

                b.HasKey("Id");

                b.HasIndex("ConsultaId")
                    .IsUnique()
                    .HasDatabaseName("IX_PRESCRICOES_CONSULTA_ID_UNIQUE");

                b.HasIndex("IdConsulta")
                    .IsUnique()
                    .HasDatabaseName("IX_PRESCRICOES_ID_CONSULTA_UNIQUE");

                b.ToTable("PJ_PRESCRICOES", (string)null);
            });

            // Tabela Medicamento
            modelBuilder.Entity("MedVet.Domain.Entities.Medicamento", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("NUMBER(10)")
                    .HasColumnName("ID");

                OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<bool>("Active")
                    .HasColumnType("NUMBER(1)")
                    .HasColumnName("ACTIVE");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("TIMESTAMP")
                    .HasColumnName("CREATED_AT");

                b.Property<string>("Fabricante")
                    .HasMaxLength(100)
                    .HasColumnType("VARCHAR2(100)")
                    .HasColumnName("FABRICANTE");

                b.Property<string>("Nome")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("VARCHAR2(100)")
                    .HasColumnName("NOME");

                b.Property<string>("PrincipioAtivo")
                    .HasMaxLength(200)
                    .HasColumnType("VARCHAR2(200)")
                    .HasColumnName("PRINCIPIO_ATIVO");

                b.HasKey("Id");

                b.ToTable("PJ_MEDICAMENTOS", (string)null);
            });

            // Tabela de junção N:N (Prescricao x Medicamento)
            modelBuilder.Entity("PJ_PRESCRICOES_MEDICAMENTOS", b =>
            {
                b.Property<int>("PrescricaoId")
                    .HasColumnType("NUMBER(10)")
                    .HasColumnName("PRESCRICAO_ID");

                b.Property<int>("MedicamentoId")
                    .HasColumnType("NUMBER(10)")
                    .HasColumnName("MEDICAMENTO_ID");

                b.Property<bool>("Active")
                    .HasColumnType("NUMBER(1)")
                    .HasColumnName("ACTIVE");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("TIMESTAMP")
                    .HasColumnName("CREATED_AT");

                b.HasKey("PrescricaoId", "MedicamentoId");

                b.HasIndex("MedicamentoId");

                b.ToTable("PJ_PRESCRICOES_MEDICAMENTOS", (string)null);
            });

            // Relacionamento: Dono -> Pet (1:N)
            modelBuilder.Entity("MedVet.Domain.Entities.Pet", b =>
            {
                b.HasOne("MedVet.Domain.Entities.Dono", "Dono")
                    .WithMany("Pets")
                    .HasForeignKey("DonoId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Relacionamento: Consulta -> Pet
            modelBuilder.Entity("MedVet.Domain.Entities.Consulta", b =>
            {
                b.HasOne("MedVet.Domain.Entities.Pet", "Pet")
                    .WithMany("Consultas")
                    .HasForeignKey("PetId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Relacionamento: Prescricao -> Consulta (1:1)
            modelBuilder.Entity("MedVet.Domain.Entities.Prescricao", b =>
            {
                b.HasOne("MedVet.Domain.Entities.Consulta", "Consulta")
                    .WithOne("Prescricoes")
                    .HasForeignKey("MedVet.Domain.Entities.Prescricao", "ConsultaId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Relacionamento N:N: Prescricao -> Medicamento
            modelBuilder.Entity("PJ_PRESCRICOES_MEDICAMENTOS", b =>
            {
                b.HasOne("MedVet.Domain.Entities.Medicamento", null)
                    .WithMany()
                    .HasForeignKey("MedicamentoId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("MedVet.Domain.Entities.Prescricao", null)
                    .WithMany()
                    .HasForeignKey("PrescricaoId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            // Navegações
            modelBuilder.Entity("MedVet.Domain.Entities.Dono", b => { b.Navigation("Pets"); });

            modelBuilder.Entity("MedVet.Domain.Entities.Pet", b => { b.Navigation("Consultas"); });

            modelBuilder.Entity("MedVet.Domain.Entities.Consulta", b => { b.Navigation("Prescricoes"); });
#pragma warning restore 612, 618
        
    }

}