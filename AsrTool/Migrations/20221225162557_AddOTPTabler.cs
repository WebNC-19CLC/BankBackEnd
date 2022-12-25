using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsrTool.Migrations
{
    public partial class AddOTPTabler : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BankAccountId",
                table: "Recipient",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OTP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAccountId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OTP_BankAccount_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipient_BankAccountId",
                table: "Recipient",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OTP_BankAccountId",
                table: "OTP",
                column: "BankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipient_BankAccount_BankAccountId",
                table: "Recipient",
                column: "BankAccountId",
                principalTable: "BankAccount",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipient_BankAccount_BankAccountId",
                table: "Recipient");

            migrationBuilder.DropTable(
                name: "OTP");

            migrationBuilder.DropIndex(
                name: "IX_Recipient_BankAccountId",
                table: "Recipient");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                table: "Recipient");
        }
    }
}
