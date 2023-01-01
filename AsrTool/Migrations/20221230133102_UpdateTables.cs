using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsrTool.Migrations
{
    public partial class UpdateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OTP_BankAccount_BankAccountId",
                table: "OTP");

            migrationBuilder.DropIndex(
                name: "IX_OTP_BankAccountId",
                table: "OTP");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                table: "OTP");

            migrationBuilder.RenameColumn(
                name: "HashPublicKey",
                table: "Bank",
                newName: "EncryptRsaPublicKey");

            migrationBuilder.RenameColumn(
                name: "EncryptPublicKey",
                table: "Bank",
                newName: "DecryptRsaPrivateKey");

            migrationBuilder.RenameColumn(
                name: "DecryptPrivateKey",
                table: "Bank",
                newName: "DecryptPublicKey");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "OTP",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredAt",
                table: "OTP",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OTP",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FromId",
                table: "Debit",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BankAccountId",
                table: "Debit",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromAccountNumber",
                table: "Debit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OTPId",
                table: "BankAccount",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BankAccount",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Debit_BankAccountId",
                table: "Debit",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_OTPId",
                table: "BankAccount",
                column: "OTPId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_UserId",
                table: "BankAccount",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccount_Employee_UserId",
                table: "BankAccount",
                column: "UserId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccount_OTP_OTPId",
                table: "BankAccount",
                column: "OTPId",
                principalTable: "OTP",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Debit_BankAccount_BankAccountId",
                table: "Debit",
                column: "BankAccountId",
                principalTable: "BankAccount",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_Employee_UserId",
                table: "BankAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_OTP_OTPId",
                table: "BankAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Debit_BankAccount_BankAccountId",
                table: "Debit");

            migrationBuilder.DropIndex(
                name: "IX_Debit_BankAccountId",
                table: "Debit");

            migrationBuilder.DropIndex(
                name: "IX_BankAccount_OTPId",
                table: "BankAccount");

            migrationBuilder.DropIndex(
                name: "IX_BankAccount_UserId",
                table: "BankAccount");

            migrationBuilder.DropColumn(
                name: "ExpiredAt",
                table: "OTP");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OTP");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                table: "Debit");

            migrationBuilder.DropColumn(
                name: "FromAccountNumber",
                table: "Debit");

            migrationBuilder.DropColumn(
                name: "OTPId",
                table: "BankAccount");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BankAccount");

            migrationBuilder.RenameColumn(
                name: "EncryptRsaPublicKey",
                table: "Bank",
                newName: "HashPublicKey");

            migrationBuilder.RenameColumn(
                name: "DecryptRsaPrivateKey",
                table: "Bank",
                newName: "EncryptPublicKey");

            migrationBuilder.RenameColumn(
                name: "DecryptPublicKey",
                table: "Bank",
                newName: "DecryptPrivateKey");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "OTP",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankAccountId",
                table: "OTP",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FromId",
                table: "Debit",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OTP_BankAccountId",
                table: "OTP",
                column: "BankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_OTP_BankAccount_BankAccountId",
                table: "OTP",
                column: "BankAccountId",
                principalTable: "BankAccount",
                principalColumn: "Id");
        }
    }
}
