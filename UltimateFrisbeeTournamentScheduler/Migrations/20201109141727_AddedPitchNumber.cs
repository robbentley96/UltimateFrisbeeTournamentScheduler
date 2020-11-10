using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateFrisbeeTournamentScheduler.Migrations
{
    public partial class AddedPitchNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PitchNumber",
                table: "Matches",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PitchNumber",
                table: "Matches");
        }
    }
}
