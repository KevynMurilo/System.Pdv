using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace System.Pdv.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertRolesColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Nome", "Descricao" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "Admin", "Administrador do sistema" },
                    { Guid.NewGuid(), "Garcom", "Garçom responsável pelo atendimento" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove os dados se necessário
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Nome",
                keyValues: new[] { "Admin", "Garcom" });
        }
    }
}
