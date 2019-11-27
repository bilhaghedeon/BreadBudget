using Microsoft.EntityFrameworkCore.Migrations;

namespace BreadBudget.Migrations
{
    public partial class ReceiptImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Receipt",
                table: "Transaction",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receipt",
                table: "Transaction");
        }
    }
}
