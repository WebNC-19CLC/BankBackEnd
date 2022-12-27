using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsrTool.Migrations
{
    public partial class UpdateTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ChargeReceiver",
                table: "Transaction",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "TransactionFee",
                table: "Transaction",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChargeReceiver",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TransactionFee",
                table: "Transaction");
        }
    }
}
