using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketWallet.Migrations
{
    public partial class AddCustomLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FunctionRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    FunctionId = table.Column<Guid>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FunctionRuns_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FunctionRuns_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Functions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("19af78d7-5ced-4abc-a2a4-cdd1fe218786"), "User sign in to application", "SignIn" },
                    { new Guid("2e9c3c39-b363-4071-8da7-1369dbcbd68a"), "User create new account in application", "SignUp" },
                    { new Guid("9439daf7-40d8-4bed-8447-c74e6ae8ccaa"), "User change his master password", "ChangeMasterPassword" },
                    { new Guid("61d96923-635e-42e8-aa19-b1c7837ed30b"), "User gets his sign in information (i.e successful login time)", "GetLoginInfo" },
                    { new Guid("eafe5933-128c-4cb5-b1d1-70665edcadd8"), "User add new password to his wallet", "AddPassword" },
                    { new Guid("3128d37d-53f2-4888-9cfa-85757aa2466d"), "User delete password from his wallet", "DeletePassword" },
                    { new Guid("7f6be03a-8d1e-40f7-afbc-0ac557efcbbc"), "User edit password in his wallet", "EditPassword" },
                    { new Guid("e2b61fd4-5cfd-4eac-837e-fcb4410dc8ca"), "Get password record without password value", "GetFullSecurityPassword" },
                    { new Guid("a0427735-ef4e-41e3-8008-959f4f85aa5e"), "Get password decrypted value", "GetPassword" },
                    { new Guid("0545ebdc-6305-4077-8e2c-4318e01b9a38"), "Get all wallet", "GetWallet" },
                    { new Guid("a8d49ccb-267a-4424-ab69-c999c72482d1"), "Share password record to other user", "SharePassword" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FunctionRuns_FunctionId",
                table: "FunctionRuns",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionRuns_UserId",
                table: "FunctionRuns",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FunctionRuns");

            migrationBuilder.DropTable(
                name: "Functions");
        }
    }
}
