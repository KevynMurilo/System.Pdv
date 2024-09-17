using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace System.Pdv.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTablePermissao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Recurso = table.Column<string>(type: "text", nullable: true),
                    Acao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissaoRole",
                columns: table => new
                {
                    PermissoesId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissaoRole", x => new { x.PermissoesId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_PermissaoRole_Permissoes_PermissoesId",
                        column: x => x.PermissoesId,
                        principalTable: "Permissoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissaoRole_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissaoRole_RolesId",
                table: "PermissaoRole",
                column: "RolesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissaoRole");

            migrationBuilder.DropTable(
                name: "Permissoes");
        }
    }
}
