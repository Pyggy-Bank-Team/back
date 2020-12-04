using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PiggyBank.Model.Migrations
{
    public partial class AddedOperationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanDate",
                schema: "Pb",
                table: "Operations");

            migrationBuilder.AddColumn<DateTime>(
                name: "OperationDate",
                schema: "Pb",
                table: "Operations",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationDate",
                schema: "Pb",
                table: "Operations");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlanDate",
                schema: "Pb",
                table: "Operations",
                type: "datetime2",
                nullable: true);
        }
    }
}
