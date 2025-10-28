using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESG.Compliance.Api.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicialTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditoriasAmbientais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DataAgendada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditorResponsavel = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriasAmbientais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LicencasAmbientais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeDaLicenca = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OrgaoEmissor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataDeEmissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDeExpiracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicencasAmbientais", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriasAmbientais");

            migrationBuilder.DropTable(
                name: "LicencasAmbientais");
        }
    }
}
