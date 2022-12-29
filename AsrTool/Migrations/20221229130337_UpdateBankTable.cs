using Microsoft.EntityFrameworkCore.Migrations;


#nullable disable

namespace AsrTool.Migrations
{
    public partial class UpdateBankTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublicKey",
                table: "Bank",
                newName: "HashPublicKey");

            migrationBuilder.RenameColumn(
                name: "PrivateKey",
                table: "Bank",
                newName: "EncryptPublicKey");

            migrationBuilder.AddColumn<string>(
                name: "DecryptPrivateKey",
                table: "Bank",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecryptPrivateKey",
                table: "Bank");

            migrationBuilder.RenameColumn(
                name: "HashPublicKey",
                table: "Bank",
                newName: "PublicKey");

            migrationBuilder.RenameColumn(
                name: "EncryptPublicKey",
                table: "Bank",
                newName: "PrivateKey");
        }
    }
}