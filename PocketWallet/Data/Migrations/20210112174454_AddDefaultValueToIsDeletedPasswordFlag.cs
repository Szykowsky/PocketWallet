using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketWallet.Migrations
{
    public partial class AddDefaultValueToIsDeletedPasswordFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Passwords",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.InsertData(
                table: "Functions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("2d204615-99f7-4fe6-ab9e-91a6e73a1293"), "User sign in to application", "SignIn" },
                    { new Guid("1624c18a-2eb7-4ba7-b943-f15daee1615a"), "User create new account in application", "SignUp" },
                    { new Guid("418342bf-1543-4cd1-b65f-00de3173949d"), "User change his master password", "ChangeMasterPassword" },
                    { new Guid("583535d2-3172-4f0c-b76d-3cce607129b3"), "User gets his sign in information (i.e successful login time)", "GetLoginInfo" },
                    { new Guid("06b4b189-04f0-4f41-93f1-9125feb56f3e"), "User add new password to his wallet", "AddPassword" },
                    { new Guid("11740cea-4e46-4e12-b503-b505f8245e29"), "User delete password from his wallet", "DeletePassword" },
                    { new Guid("5ab40cbd-4bd2-4107-8a04-a68c58a3fac0"), "User edit password in his wallet", "EditPassword" },
                    { new Guid("26dd9061-a013-41c2-a9b7-b03c4b083c1a"), "Get password record without password value", "GetFullSecurityPassword" },
                    { new Guid("d8588a4a-965f-4617-8ec6-ff4ec7caffaa"), "Get password decrypted value", "GetPassword" },
                    { new Guid("f856af0a-3a5c-41d7-a3a7-55f251ffc200"), "Get all wallet", "GetWallet" },
                    { new Guid("f03b0030-e102-4fa5-9261-ff2a66786eab"), "Share password record to other user", "SharePassword" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("06b4b189-04f0-4f41-93f1-9125feb56f3e"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("11740cea-4e46-4e12-b503-b505f8245e29"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("1624c18a-2eb7-4ba7-b943-f15daee1615a"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("26dd9061-a013-41c2-a9b7-b03c4b083c1a"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("2d204615-99f7-4fe6-ab9e-91a6e73a1293"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("418342bf-1543-4cd1-b65f-00de3173949d"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("583535d2-3172-4f0c-b76d-3cce607129b3"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("5ab40cbd-4bd2-4107-8a04-a68c58a3fac0"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("d8588a4a-965f-4617-8ec6-ff4ec7caffaa"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("f03b0030-e102-4fa5-9261-ff2a66786eab"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("f856af0a-3a5c-41d7-a3a7-55f251ffc200"));

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Passwords",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

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
        }
    }
}
