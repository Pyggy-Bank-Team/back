using Microsoft.EntityFrameworkCore.Migrations;

namespace PiggyBank.Model.Migrations
{
    public partial class AddedSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Source",
                schema: "Pb",
                table: "Operations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                schema: "Pb",
                table: "Operations");
        }
    }
}
