using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace System.Pdv.Infrastructure.Migrations
{
    public partial class AddAdminRolePermissions : Migration
    {
        private readonly Guid adminRoleId = new Guid("D9A0C2D6-4A28-4F92-ABCD-0981A7A3F9E6");

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adiciona permissões para cada rota e ação
            string[] recursos = new string[]
            {
                "Adicional", "Categoria", "Cliente", "Mesa", "MetodoPagamento",
                "Pedido", "Produto", "Role", "RolePermission", "StatusPedido", "Usuario"
            };

            string[] acoes = new string[] { "Create", "Update", "Delete", "Get" };

            foreach (var recurso in recursos)
            {
                foreach (var acao in acoes)
                {
                    Guid permissaoId = Guid.NewGuid(); // Gera um novo Guid para a permissão

                    // Insere a permissão na tabela Permissoes
                    migrationBuilder.InsertData(
                        table: "Permissoes",
                        columns: new[] { "Id", "Recurso", "Acao" },
                        values: new object[] { permissaoId, recurso, acao });

                    // Associa a permissão à role ADMIN
                    migrationBuilder.InsertData(
                        table: "PermissaoRole",
                        columns: new[] { "PermissoesId", "RolesId" },
                        values: new object[] { permissaoId, adminRoleId });
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove as permissões inseridas na tabela PermissaoRole e Permissoes
            string[] recursos = new string[]
            {
                "Adicional", "Categoria", "Cliente", "Mesa", "MetodoPagamento",
                "Pedido", "Produto", "Role", "RolePermission", "StatusPedido", "Usuario"
            };

            string[] acoes = new string[] { "Create", "Update", "Delete", "Get" };

            foreach (var recurso in recursos)
            {
                foreach (var acao in acoes)
                {
                    // Remova as associações de permissões da tabela PermissaoRole
                    migrationBuilder.Sql($@"
                        DELETE FROM ""PermissaoRole"" 
                        WHERE ""RolesId"" = '{adminRoleId}'
                        AND ""PermissoesId"" = (
                            SELECT ""Id"" FROM ""Permissoes"" 
                            WHERE ""Recurso"" = '{recurso}' AND ""Acao"" = '{acao}'
                        )");

                    // Remova as permissões da tabela Permissoes
                    migrationBuilder.Sql($@"
                        DELETE FROM ""Permissoes"" 
                        WHERE ""Recurso"" = '{recurso}' AND ""Acao"" = '{acao}'
                    ");
                }
            }
        }
    }
}
