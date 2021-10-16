using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PiggyBank.Model.Migrations
{
    public partial class AddedBotOperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BotOperations",
                schema: "Pb",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<Guid>(nullable: true),
                    ChatId = table.Column<long>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Stage = table.Column<int>(nullable: false),
                    CategoryType = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    AccountId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    ToAccountId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotOperations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotOperations",
                schema: "Pb");
        }
    }
}
