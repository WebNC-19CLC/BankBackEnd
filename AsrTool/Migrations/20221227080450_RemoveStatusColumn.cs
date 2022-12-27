using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsrTool.Migrations
{
    public partial class RemoveStatusColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "BankAccount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BankAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
