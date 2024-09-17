using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace System.Pdv.Infrastructure.Migrations
{
    public partial class AddGarcomRolePermissions : Migration
    {
        private readonly Guid garcomRoleId = new Guid("B2C0D3E6-7F28-4B9E-9CDE-1234F56789AB");

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Permissões específicas para GARCOM
            var permissoes = new (string Recurso, string Acao)[]
            {
                ("Adicional", "Get"),
                ("Categoria", "Get"),
                ("Mesa", "Get"),
                ("MetodoPagamento", "Get"),
                ("Pedido", "Get"),
                ("Pedido", "Post"),
                ("Pedido", "Update"),
                ("Pedido", "Delete"),
                ("Produto", "Get"),
                ("StatusPedido", "Get")
            };

            foreach (var (recurso, acao) in permissoes)
            {
                Guid permissaoId = Guid.NewGuid();

                // Insere a permissão na tabela Permissoes
                migrationBuilder.InsertData(
                    table: "Permissoes",
                    columns: new[] { "Id", "Recurso", "Acao" },
                    values: new object[] { permissaoId, recurso, acao });

                // Associa a permissão à role GARCOM
                migrationBuilder.InsertData(
                    table: "PermissaoRole",
                    columns: new[] { "PermissoesId", "RolesId" },
                    values: new object[] { permissaoId, garcomRoleId });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove as permissões inseridas na tabela PermissaoRole e Permissoes
            var permissoes = new (string Recurso, string Acao)[]
            {
                ("Adicional", "Get"),
                ("Categoria", "Get"),
                ("Mesa", "Get"),
                ("MetodoPagamento", "Get"),
                ("Pedido", "Get"),
                ("Pedido", "Post"),
                ("Pedido", "Update"),
                ("Pedido", "Delete"),
                ("Produto", "Get"),
                ("StatusPedido", "Get")
            };

            foreach (var (recurso, acao) in permissoes)
            {
                // Remove as associações de permissões da tabela PermissaoRole
                migrationBuilder.Sql($@"
                    DELETE FROM ""PermissaoRole""
                    WHERE ""RolesId"" = '{garcomRoleId}'
                    AND ""PermissoesId"" = (
                        SELECT ""Id"" FROM ""Permissoes""
                        WHERE ""Recurso"" = '{recurso}' AND ""Acao"" = '{acao}'
                    )");

                // Remove as permissões da tabela Permissoes
                migrationBuilder.Sql($@"
                    DELETE FROM ""Permissoes""
                    WHERE ""Recurso"" = '{recurso}' AND ""Acao"" = '{acao}'
                ");
            }
        }
    }
}
