using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketWallet.Migrations
{
    public partial class AddPasswordDataChangesFollowing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("0545ebdc-6305-4077-8e2c-4318e01b9a38"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("19af78d7-5ced-4abc-a2a4-cdd1fe218786"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("2e9c3c39-b363-4071-8da7-1369dbcbd68a"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("3128d37d-53f2-4888-9cfa-85757aa2466d"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("61d96923-635e-42e8-aa19-b1c7837ed30b"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("7f6be03a-8d1e-40f7-afbc-0ac557efcbbc"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("9439daf7-40d8-4bed-8447-c74e6ae8ccaa"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("a0427735-ef4e-41e3-8008-959f4f85aa5e"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("a8d49ccb-267a-4424-ab69-c999c72482d1"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("e2b61fd4-5cfd-4eac-837e-fcb4410dc8ca"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("eafe5933-128c-4cb5-b1d1-70665edcadd8"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Passwords",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DataChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    RecordId = table.Column<Guid>(nullable: false),
                    PreviousValue = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ActionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataChanges_Users_UserId",
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
                    { new Guid("37f7030b-ad22-4c52-9773-76422a98e9a1"), "User sign in to application", "SignIn" },
                    { new Guid("9e9c176d-3bbe-4d05-873f-17f230f8227a"), "User create new account in application", "SignUp" },
                    { new Guid("f8c1ae21-dd92-42a0-8ba4-281072d117b0"), "User change his master password", "ChangeMasterPassword" },
                    { new Guid("4ac20451-b2be-4aae-b94e-702991e26745"), "User gets his sign in information (i.e successful login time)", "GetLoginInfo" },
                    { new Guid("6a67ea1e-de16-4d36-8f93-2d3e784fbd2c"), "User add new password to his wallet", "AddPassword" },
                    { new Guid("9dfb6cf9-1af1-4b40-8c88-eeed792434cc"), "User delete password from his wallet", "DeletePassword" },
                    { new Guid("ed309dfd-6b62-4286-941b-bdfa0d44a558"), "User edit password in his wallet", "EditPassword" },
                    { new Guid("5e2f9876-d490-48f5-ba25-f76ee53ffe7e"), "Get password record without password value", "GetFullSecurityPassword" },
                    { new Guid("b309edf2-8de9-40c6-a7ce-5404248c3de8"), "Get password decrypted value", "GetPassword" },
                    { new Guid("b961ef2f-0b08-49c7-a9dc-1e1ddbe42bcf"), "Get all wallet", "GetWallet" },
                    { new Guid("d4bfbdf7-95de-417b-91a4-778f5cfab321"), "Share password record to other user", "SharePassword" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataChanges_UserId",
                table: "DataChanges",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataChanges");

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("37f7030b-ad22-4c52-9773-76422a98e9a1"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("4ac20451-b2be-4aae-b94e-702991e26745"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("5e2f9876-d490-48f5-ba25-f76ee53ffe7e"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("6a67ea1e-de16-4d36-8f93-2d3e784fbd2c"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("9dfb6cf9-1af1-4b40-8c88-eeed792434cc"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("9e9c176d-3bbe-4d05-873f-17f230f8227a"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("b309edf2-8de9-40c6-a7ce-5404248c3de8"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("b961ef2f-0b08-49c7-a9dc-1e1ddbe42bcf"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("d4bfbdf7-95de-417b-91a4-778f5cfab321"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("ed309dfd-6b62-4286-941b-bdfa0d44a558"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("f8c1ae21-dd92-42a0-8ba4-281072d117b0"));

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Passwords");

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
        }
    }
}
