using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreApiSample.Database.Migrations
{
    public partial class ExpiracaoSenha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataHoraUltimaAlteracaoSenha",
                table: "Usuario",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<bool>(
                name: "ExpiracaoSenhaAtivada",
                table: "Usuario",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataHoraUltimaAlteracaoSenha",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "ExpiracaoSenhaAtivada",
                table: "Usuario");
        }
    }
}
