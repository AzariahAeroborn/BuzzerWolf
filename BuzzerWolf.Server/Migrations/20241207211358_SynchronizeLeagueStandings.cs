using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuzzerWolf.Server.Migrations
{
    /// <inheritdoc />
    public partial class SynchronizeLeagueStandings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeagueStandings",
                columns: table => new
                {
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conference = table.Column<int>(type: "int", nullable: false),
                    ConferenceRank = table.Column<int>(type: "int", nullable: false),
                    Wins = table.Column<int>(type: "int", nullable: false),
                    Losses = table.Column<int>(type: "int", nullable: false),
                    PointsFor = table.Column<int>(type: "int", nullable: false),
                    PointsAgainst = table.Column<int>(type: "int", nullable: false),
                    IsBot = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueStandings", x => new { x.LeagueId, x.Season, x.TeamId });
                    table.ForeignKey(
                        name: "FK_LeagueStandings_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueStandings");
        }
    }
}
