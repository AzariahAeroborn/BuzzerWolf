using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuzzerWolf.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Divisions = table.Column<int>(type: "INTEGER", nullable: true),
                    FirstSeason = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromotionStandings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Country = table.Column<int>(type: "INTEGER", nullable: false),
                    Division = table.Column<int>(type: "INTEGER", nullable: false),
                    Season = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamName = table.Column<string>(type: "TEXT", nullable: false),
                    Wins = table.Column<int>(type: "INTEGER", nullable: false),
                    Losses = table.Column<int>(type: "INTEGER", nullable: false),
                    PointDifference = table.Column<int>(type: "INTEGER", nullable: false),
                    ConferenceRank = table.Column<int>(type: "INTEGER", nullable: false),
                    LeagueName = table.Column<string>(type: "TEXT", nullable: false),
                    ConferenceName = table.Column<string>(type: "TEXT", nullable: false),
                    PromotionRank = table.Column<int>(type: "INTEGER", nullable: false),
                    IsChampionPromotion = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAutoPromotion = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsBotPromotion = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsTotalPromotion = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionStandings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Start = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Finish = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sync",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: true),
                    Table = table.Column<string>(type: "TEXT", nullable: false),
                    Params = table.Column<string>(type: "TEXT", nullable: true),
                    LastSync = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    NextAutoSync = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sync", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "PromotionStandings");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Sync");
        }
    }
}
