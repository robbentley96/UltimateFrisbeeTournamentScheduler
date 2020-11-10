using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateFrisbeeTournamentScheduler.Migrations
{
    public partial class AddedStartTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phase",
                table: "Tournaments");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartingTime",
                table: "Tournaments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartingTime",
                table: "Tournaments");

            migrationBuilder.AddColumn<string>(
                name: "Phase",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
