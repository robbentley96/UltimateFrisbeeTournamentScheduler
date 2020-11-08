using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateFrisbeeTournamentScheduler.Migrations
{
    public partial class AddedPoolFunctionality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pool",
                table: "Teams");

            migrationBuilder.AddColumn<int>(
                name: "PoolId",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pools",
                columns: table => new
                {
                    PoolId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pools", x => x.PoolId);
                    table.ForeignKey(
                        name: "FK_Pools_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_PoolId",
                table: "Teams",
                column: "PoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Pools_TournamentId",
                table: "Pools",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Pools_PoolId",
                table: "Teams",
                column: "PoolId",
                principalTable: "Pools",
                principalColumn: "PoolId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Pools_PoolId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "Pools");

            migrationBuilder.DropIndex(
                name: "IX_Teams_PoolId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "PoolId",
                table: "Teams");

            migrationBuilder.AddColumn<int>(
                name: "Pool",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
