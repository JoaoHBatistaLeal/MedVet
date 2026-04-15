using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedVet.Infrastructre.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PJ_DONOS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR2(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR2(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "VARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PJ_DONOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PJ_MEDICAMENTOS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NomeMedicamento = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Marca = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ModoDeUso = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Preco = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PJ_MEDICAMENTOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PJ_PRESCRICOES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ID_CONSULTA = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PJ_PRESCRICOES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PJ_VETERINARIOS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR2(200)", maxLength: 200, nullable: false),
                    CRMV = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Especialidade = table.Column<string>(type: "VARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PJ_VETERINARIOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PJ_ANIMAIS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ID_DONO = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR2(100)", maxLength: 100, nullable: false),
                    TIPO_ANIMAL = table.Column<string>(type: "VARCHAR2(50)", maxLength: 50, nullable: false),
                    Raca = table.Column<string>(type: "VARCHAR2(50)", maxLength: 50, nullable: false),
                    Genero = table.Column<string>(type: "VARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PJ_ANIMAIS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PJ_ANIMAIS_PJ_DONOS_ID_DONO",
                        column: x => x.ID_DONO,
                        principalTable: "PJ_DONOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PJ_PRESCRICOES_MEDICAMENTOS",
                columns: table => new
                {
                    PrescricaoId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    MedicamentoId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Active = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    IdMedicamento = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    IdPrescricao = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PJ_PRESCRICOES_MEDICAMENTOS", x => new { x.PrescricaoId, x.MedicamentoId });
                    table.ForeignKey(
                        name: "FK_PJ_PRESCRICOES_MEDICAMENTOS_PJ_MEDICAMENTOS_IdMedicamento",
                        column: x => x.IdMedicamento,
                        principalTable: "PJ_MEDICAMENTOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PJ_PRESCRICOES_MEDICAMENTOS_PJ_PRESCRICOES_IdPrescricao",
                        column: x => x.IdPrescricao,
                        principalTable: "PJ_PRESCRICOES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PJ_CONSULTAS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ID_PET = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ID_VETERINARIO = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    DATA_CONSULTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Diagnostico = table.Column<string>(type: "VARCHAR2(500)", maxLength: 500, nullable: false),
                    Observacoes = table.Column<string>(type: "VARCHAR2(1000)", maxLength: 1000, nullable: false),
                    PrescricoesId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PJ_CONSULTAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PJ_CONSULTAS_PJ_ANIMAIS_ID_PET",
                        column: x => x.ID_PET,
                        principalTable: "PJ_ANIMAIS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PJ_CONSULTAS_PJ_PRESCRICOES_PrescricoesId",
                        column: x => x.PrescricoesId,
                        principalTable: "PJ_PRESCRICOES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PJ_CONSULTAS_PJ_VETERINARIOS_ID_VETERINARIO",
                        column: x => x.ID_VETERINARIO,
                        principalTable: "PJ_VETERINARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ANIMAIS_ID_DONO",
                table: "PJ_ANIMAIS",
                column: "ID_DONO");

            migrationBuilder.CreateIndex(
                name: "IX_ANIMAIS_NOME",
                table: "PJ_ANIMAIS",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_ANIMAIS_RACA",
                table: "PJ_ANIMAIS",
                column: "Raca");

            migrationBuilder.CreateIndex(
                name: "IX_ANIMAIS_TIPO_ANIMAL",
                table: "PJ_ANIMAIS",
                column: "TIPO_ANIMAL");

            migrationBuilder.CreateIndex(
                name: "IX_CONSULTAS_DATA_CONSULTA",
                table: "PJ_CONSULTAS",
                column: "DATA_CONSULTA");

            migrationBuilder.CreateIndex(
                name: "IX_CONSULTAS_ID_PET",
                table: "PJ_CONSULTAS",
                column: "ID_PET");

            migrationBuilder.CreateIndex(
                name: "IX_CONSULTAS_ID_VETERINARIO",
                table: "PJ_CONSULTAS",
                column: "ID_VETERINARIO");

            migrationBuilder.CreateIndex(
                name: "IX_CONSULTAS_PET_DATA",
                table: "PJ_CONSULTAS",
                columns: new[] { "ID_PET", "DATA_CONSULTA" });

            migrationBuilder.CreateIndex(
                name: "IX_PJ_CONSULTAS_PrescricoesId",
                table: "PJ_CONSULTAS",
                column: "PrescricoesId");

            migrationBuilder.CreateIndex(
                name: "IX_DONOS_EMAIL",
                table: "PJ_DONOS",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_DONOS_NOME",
                table: "PJ_DONOS",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_PRESCRICOES_ID_CONSULTA_UNIQUE",
                table: "PJ_PRESCRICOES",
                column: "ID_CONSULTA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PJ_PRESCRICOES_MEDICAMENTOS_IdMedicamento",
                table: "PJ_PRESCRICOES_MEDICAMENTOS",
                column: "IdMedicamento");

            migrationBuilder.CreateIndex(
                name: "IX_PJ_PRESCRICOES_MEDICAMENTOS_IdPrescricao",
                table: "PJ_PRESCRICOES_MEDICAMENTOS",
                column: "IdPrescricao");

            migrationBuilder.CreateIndex(
                name: "IX_VETERINARIOS_CRMV_UNIQUE",
                table: "PJ_VETERINARIOS",
                column: "CRMV",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VETERINARIOS_ESPECIALIDADE",
                table: "PJ_VETERINARIOS",
                column: "Especialidade");

            migrationBuilder.CreateIndex(
                name: "IX_VETERINARIOS_NOME",
                table: "PJ_VETERINARIOS",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_VETERINARIOS_NOME_ESPECIALIDADE",
                table: "PJ_VETERINARIOS",
                columns: new[] { "Nome", "Especialidade" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PJ_CONSULTAS");

            migrationBuilder.DropTable(
                name: "PJ_PRESCRICOES_MEDICAMENTOS");

            migrationBuilder.DropTable(
                name: "PJ_ANIMAIS");

            migrationBuilder.DropTable(
                name: "PJ_VETERINARIOS");

            migrationBuilder.DropTable(
                name: "PJ_MEDICAMENTOS");

            migrationBuilder.DropTable(
                name: "PJ_PRESCRICOES");

            migrationBuilder.DropTable(
                name: "PJ_DONOS");
        }
    }
}
