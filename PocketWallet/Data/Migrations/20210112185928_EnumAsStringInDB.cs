using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketWallet.Migrations
{
    public partial class EnumAsStringInDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "ActionType",
                table: "DataChanges",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Functions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("82faf31b-b754-41d2-9ac2-742643f96503"), "User sign in to application", "SignIn" },
                    { new Guid("660cea17-8c94-4d2b-add4-655514d8f998"), "User create new account in application", "SignUp" },
                    { new Guid("cef9db47-dbb8-4655-a741-1cc394007e0c"), "User change his master password", "ChangeMasterPassword" },
                    { new Guid("8fea7f6e-54d3-4915-ae34-7819bb980ca1"), "User gets his sign in information (i.e successful login time)", "GetLoginInfo" },
                    { new Guid("cd1e512d-baa6-41f6-95b9-20000ad7e279"), "User add new password to his wallet", "AddPassword" },
                    { new Guid("bdecbb1e-9d0f-4d32-9650-e09f5aa21fc2"), "User delete password from his wallet", "DeletePassword" },
                    { new Guid("3264732d-3ac6-42ba-af99-5841b6d830ed"), "User edit password in his wallet", "EditPassword" },
                    { new Guid("19ac49d8-1b0d-4c16-a89e-9efabfb165d9"), "Get password record without password value", "GetFullSecurityPassword" },
                    { new Guid("82e8c449-4ad7-4067-b181-05447c161fb3"), "Get password decrypted value", "GetPassword" },
                    { new Guid("7716e6a3-43d3-4a72-9aea-179fb60a7a57"), "Get all wallet", "GetWallet" },
                    { new Guid("ba46cc4f-25fd-4efb-8b12-7dce0032a57c"), "Share password record to other user", "SharePassword" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("19ac49d8-1b0d-4c16-a89e-9efabfb165d9"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("3264732d-3ac6-42ba-af99-5841b6d830ed"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("660cea17-8c94-4d2b-add4-655514d8f998"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("7716e6a3-43d3-4a72-9aea-179fb60a7a57"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("82e8c449-4ad7-4067-b181-05447c161fb3"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("82faf31b-b754-41d2-9ac2-742643f96503"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("8fea7f6e-54d3-4915-ae34-7819bb980ca1"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("ba46cc4f-25fd-4efb-8b12-7dce0032a57c"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("bdecbb1e-9d0f-4d32-9650-e09f5aa21fc2"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("cd1e512d-baa6-41f6-95b9-20000ad7e279"));

            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "Id",
                keyValue: new Guid("cef9db47-dbb8-4655-a741-1cc394007e0c"));

            migrationBuilder.AlterColumn<int>(
                name: "ActionType",
                table: "DataChanges",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

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
    }
}
