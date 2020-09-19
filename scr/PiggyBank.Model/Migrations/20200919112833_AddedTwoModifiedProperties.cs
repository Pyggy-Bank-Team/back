using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PiggyBank.Model.Migrations
{
    public partial class AddedTwoModifiedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Shapshot",
                table: "Operations",
                newName:"Snapshot",
                schema: "Pb"
            );

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                schema: "Pb",
                table: "Operations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "Pb",
                table: "Operations",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                schema: "Pb",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "Pb",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                schema: "Pb",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "Pb",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "Pb",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "Pb",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "Snapshot",
                schema: "Pb",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "Pb",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "Pb",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "Pb",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "Pb",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "Snapshot",
                table: "Operations",
                newName:"Shapshot",
                schema: "Pb"
            );
        }
    }
}
