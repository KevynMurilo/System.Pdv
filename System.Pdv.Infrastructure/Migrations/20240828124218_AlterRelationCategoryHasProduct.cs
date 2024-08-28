using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace System.Pdv.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterRelationCategoryHasProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriaProduto");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoriaId",
                table: "Produtos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaId",
                table: "Produtos",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Categorias_CategoriaId",
                table: "Produtos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Categorias_CategoriaId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_CategoriaId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Produtos");

            migrationBuilder.CreateTable(
                name: "CategoriaProduto",
                columns: table => new
                {
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProdutosId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaProduto", x => new { x.CategoriaId, x.ProdutosId });
                    table.ForeignKey(
                        name: "FK_CategoriaProduto_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaProduto_Produtos_ProdutosId",
                        column: x => x.ProdutosId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaProduto_ProdutosId",
                table: "CategoriaProduto",
                column: "ProdutosId");
        }
    }
}
