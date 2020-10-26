using Microsoft.EntityFrameworkCore.Migrations;

namespace PiggyBank.IdentityServer.Migrations
{
    public partial class AddCurrencyBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyBase",
                schema: "Idt",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyBase",
                schema: "Idt",
                table: "Users");
        }
    }
}
