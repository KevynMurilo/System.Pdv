using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace System.Pdv.Infrastructure.Migrations
{
    public partial class AddRolesPermissions : Migration
    {
        private readonly Guid adminRoleId = new Guid("D9A0C2D6-4A28-4F92-ABCD-0981A7A3F9E6");
        private readonly Guid garcomRoleId = new Guid("B2C0D3E6-7F28-4B9E-9CDE-1234F56789AB");

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Defina permissões para ADMIN
            var adminPermissoes = new (string Recurso, string Acao)[]
            {
                ("Adicional", "Create"), ("Adicional", "Update"), ("Adicional", "Delete"), ("Adicional", "Get"),
                ("Categoria", "Create"), ("Categoria", "Update"), ("Categoria", "Delete"), ("Categoria", "Get"),
                ("Cliente", "Create"), ("Cliente", "Update"), ("Cliente", "Delete"), ("Cliente", "Get"),
                ("Mesa", "Create"), ("Mesa", "Update"), ("Mesa", "Delete"), ("Mesa", "Get"),
                ("MetodoPagamento", "Create"), ("MetodoPagamento", "Update"), ("MetodoPagamento", "Delete"), ("MetodoPagamento", "Get"),
                ("Pedido", "Create"), ("Pedido", "Update"), ("Pedido", "Delete"), ("Pedido", "Get"),
                ("Produto", "Create"), ("Produto", "Update"), ("Produto", "Delete"), ("Produto", "Get"),
                ("Role", "Create"), ("Role", "Update"), ("Role", "Delete"), ("Role", "Get"),
                ("RolePermission", "Create"), ("RolePermission", "Update"), ("RolePermission", "Delete"), ("RolePermission", "Get"),
                ("StatusPedido", "Create"), ("StatusPedido", "Update"), ("StatusPedido", "Delete"), ("StatusPedido", "Get"),
                ("Usuario", "Create"), ("Usuario", "Update"), ("Usuario", "Delete"), ("Usuario", "Get"),
                ("Printer", "Create"), ("Printer", "Get") 
            };

            // Defina permissões para GARÇOM
            var garcomPermissoes = new (string Recurso, string Acao)[]
            {
                ("Adicional", "Get"), ("Categoria", "Get"), ("Mesa", "Get"), ("MetodoPagamento", "Get"),
                ("Pedido", "Create"), ("Pedido", "Update"), ("Pedido", "Delete"), ("Pedido", "Get"),
                ("Produto", "Get"), ("StatusPedido", "Get"),
                ("Printer", "Create"), ("Printer", "Get") 
            };

            // Insere as permissões e associa às roles
            InserirPermissoesERoles(migrationBuilder, adminPermissoes, adminRoleId);
            InserirPermissoesERoles(migrationBuilder, garcomPermissoes, garcomRoleId);
        }

        private void InserirPermissoesERoles(MigrationBuilder migrationBuilder, (string Recurso, string Acao)[] permissoes, Guid roleId)
        {
            foreach (var (recurso, acao) in permissoes)
            {
                // Verifica se a permissão já existe
                migrationBuilder.Sql($@"
                    INSERT INTO ""Permissoes"" (""Id"", ""Recurso"", ""Acao"")
                    SELECT '{Guid.NewGuid()}', '{recurso}', '{acao}'
                    WHERE NOT EXISTS (
                        SELECT 1 FROM ""Permissoes""
                        WHERE ""Recurso"" = '{recurso}' AND ""Acao"" = '{acao}'
                    );");

                // Associa a permissão à role
                migrationBuilder.Sql($@"
                    INSERT INTO ""PermissaoRole"" (""PermissoesId"", ""RolesId"")
                    SELECT p.""Id"", '{roleId}'
                    FROM ""Permissoes"" p
                    WHERE p.""Recurso"" = '{recurso}' AND p.""Acao"" = '{acao}'
                    AND NOT EXISTS (
                        SELECT 1 FROM ""PermissaoRole"" pr
                        WHERE pr.""PermissoesId"" = p.""Id"" AND pr.""RolesId"" = '{roleId}'
                    );");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover as permissões associadas ao ADMIN e GARÇOM
            var permissoes = new (string Recurso, string Acao)[]
            {
                ("Adicional", "Get"), ("Adicional", "Create"), ("Adicional", "Update"), ("Adicional", "Delete"),
                ("Categoria", "Get"), ("Categoria", "Create"), ("Categoria", "Update"), ("Categoria", "Delete"),
                ("Cliente", "Get"), ("Cliente", "Create"), ("Cliente", "Update"), ("Cliente", "Delete"),
                ("Mesa", "Get"), ("Mesa", "Create"), ("Mesa", "Update"), ("Mesa", "Delete"),
                ("MetodoPagamento", "Get"), ("MetodoPagamento", "Create"), ("MetodoPagamento", "Update"), ("MetodoPagamento", "Delete"),
                ("Pedido", "Get"), ("Pedido", "Create"), ("Pedido", "Update"), ("Pedido", "Delete"),
                ("Produto", "Get"), ("Produto", "Create"), ("Produto", "Update"), ("Produto", "Delete"),
                ("Role", "Get"), ("Role", "Create"), ("Role", "Update"), ("Role", "Delete"),
                ("RolePermission", "Get"), ("RolePermission", "Create"), ("RolePermission", "Update"), ("RolePermission", "Delete"),
                ("StatusPedido", "Get"), ("StatusPedido", "Create"), ("StatusPedido", "Update"), ("StatusPedido", "Delete"),
                ("Usuario", "Get"), ("Usuario", "Create"), ("Usuario", "Update"), ("Usuario", "Delete"),
                ("Printer", "Create"), ("Printer", "Get")
            };

            foreach (var (recurso, acao) in permissoes)
            {
                migrationBuilder.Sql($@"
                    DELETE FROM ""PermissaoRole""
                    WHERE ""RolesId"" IN ('{adminRoleId}', '{garcomRoleId}')
                    AND ""PermissoesId"" = (
                        SELECT ""Id"" FROM ""Permissoes""
                        WHERE ""Recurso"" = '{recurso}' AND ""Acao"" = '{acao}'
                    );");

                migrationBuilder.Sql($@"
                    DELETE FROM ""Permissoes""
                    WHERE ""Recurso"" = '{recurso}' AND ""Acao"" = '{acao}';");
            }
        }
    }
}
