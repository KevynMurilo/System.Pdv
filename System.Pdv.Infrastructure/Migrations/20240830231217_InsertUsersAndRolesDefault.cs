using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace System.Pdv.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertUsersAndRolesDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserir papéis (Roles) na tabela "Roles"
            var adminRoleId = Guid.NewGuid();
            var garcomRoleId = Guid.NewGuid();

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Nome", "Descricao" },
                values: new object[,]
                {
                    { adminRoleId, "ADMIN", "Administrador do sistema" },
                    { garcomRoleId, "GARCOM", "Garçom responsável pelo atendimento" }
                });

            // Inserir usuários na tabela "Usuarios"
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "RoleId", "Nome", "Email", "PasswordHash" },
                values: new object[,]
                {
                    { Guid.NewGuid(), adminRoleId, "UserAdmin", "admin@pdv.com", "$2a$11$xjRsDYY2V3GTJwuqdXDPFuRZrWZiOAIBTVf2IcRNBUt9wksJOnKRq" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover os dados se necessário
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Email",
                keyValues: new[] { "admin@pdv.com" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Nome",
                keyValues: new[] { "Admin", "Garcom" });
        }
    }
}
