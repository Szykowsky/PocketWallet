using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketWallet.Migrations
{
    public partial class AddSharedPasswords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SharedPasswords",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    PasswordId = table.Column<Guid>(nullable: false),
                    PasswordId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedPasswords", x => new { x.PasswordId, x.UserId });
                    table.ForeignKey(
                    name: "FK_SharedPasswords_Passwords_PasswordId",
                    column: x => x.PasswordId,
                    principalTable: "Passwords",
                    principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SharedPasswords_Passwords_PasswordId1",
                        column: x => x.PasswordId1,
                        principalTable: "Passwords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SharedPasswords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedPasswords_PasswordId1",
                table: "SharedPasswords",
                column: "PasswordId1");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPasswords_UserId",
                table: "SharedPasswords",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedPasswords");
        }
    }
}
