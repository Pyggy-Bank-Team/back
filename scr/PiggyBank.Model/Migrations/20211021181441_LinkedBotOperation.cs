using Microsoft.EntityFrameworkCore.Migrations;

namespace PiggyBank.Model.Migrations
{
    public partial class LinkedBotOperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BotOperationId",
                schema: "Pb",
                table: "Operations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_BotOperationId",
                schema: "Pb",
                table: "Operations",
                column: "BotOperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_BotOperations_BotOperationId",
                schema: "Pb",
                table: "Operations",
                column: "BotOperationId",
                principalSchema: "Pb",
                principalTable: "BotOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_BotOperations_BotOperationId",
                schema: "Pb",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_BotOperationId",
                schema: "Pb",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "BotOperationId",
                schema: "Pb",
                table: "Operations");
        }
    }
}
