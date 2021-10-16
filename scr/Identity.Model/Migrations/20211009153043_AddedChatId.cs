using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.Model.Migrations
{
    public partial class AddedChatId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChatId",
                schema: "Idt",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                schema: "Idt",
                table: "Users");
        }
    }
}
