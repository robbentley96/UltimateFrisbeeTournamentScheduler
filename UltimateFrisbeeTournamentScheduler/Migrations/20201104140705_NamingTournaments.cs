using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateFrisbeeTournamentScheduler.Migrations
{
    public partial class NamingTournaments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tournaments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tournaments");
        }
    }
}
