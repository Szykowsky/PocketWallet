using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketWallet.Migrations
{
    public partial class AddBlockForIpAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.AddColumn<DateTime>(
                name: "BlockLoginTo",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InCorrectLoginCount",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SuccessfulLogin",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UnSuccessfulLogin",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "IpAddresses",
                columns: table => new
                {
                    FromIpAddress = table.Column<string>(nullable: false),
                    IncorrectSignInCount = table.Column<int>(nullable: false),
                    IsPermanentlyBlocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpAddresses", x => x.FromIpAddress);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IpAddresses");

            migrationBuilder.DropColumn(
                name: "BlockLoginTo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InCorrectLoginCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SuccessfulLogin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UnSuccessfulLogin",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlockLoginTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InCorrectLoginCount = table.Column<int>(type: "int", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuccessfulLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnSuccessfulLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");
        }
    }
}
