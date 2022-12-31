using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsrTool.Migrations
{
    public partial class UpdateDebitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BankSourceId",
                table: "Debit",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDue",
                table: "Debit",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Debit",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Debit_BankSourceId",
                table: "Debit",
                column: "BankSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Debit_Bank_BankSourceId",
                table: "Debit",
                column: "BankSourceId",
                principalTable: "Bank",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debit_Bank_BankSourceId",
                table: "Debit");

            migrationBuilder.DropIndex(
                name: "IX_Debit_BankSourceId",
                table: "Debit");

            migrationBuilder.DropColumn(
                name: "BankSourceId",
                table: "Debit");

            migrationBuilder.DropColumn(
                name: "DateDue",
                table: "Debit");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Debit");
        }
    }
}
