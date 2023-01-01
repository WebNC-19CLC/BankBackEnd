using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsrTool.Migrations
{
    public partial class UpdateTransactionTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<int>(
                name: "FromId",
                table: "Transaction",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BankSourceId",
                table: "Transaction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromAccountNumber",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BankSourceId",
                table: "Transaction",
                column: "BankSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Bank_BankSourceId",
                table: "Transaction",
                column: "BankSourceId",
                principalTable: "Bank",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Bank_BankSourceId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_BankSourceId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "BankSourceId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "FromAccountNumber",
                table: "Transaction");

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

            migrationBuilder.AlterColumn<int>(
                name: "FromId",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
