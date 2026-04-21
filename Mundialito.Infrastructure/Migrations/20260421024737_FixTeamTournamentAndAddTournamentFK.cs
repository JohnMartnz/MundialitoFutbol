using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mundialito.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTeamTournamentAndAddTournamentFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamTournaments",
                table: "TeamTournaments");

            migrationBuilder.DropIndex(
                name: "IX_TeamTournaments_TournamentId",
                table: "TeamTournaments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamTournaments",
                table: "TeamTournaments",
                columns: new[] { "TournamentId", "TeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentId",
                table: "Matches",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Tournaments_TournamentId",
                table: "Matches",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Tournaments_TournamentId",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamTournaments",
                table: "TeamTournaments");

            migrationBuilder.DropIndex(
                name: "IX_Matches_TournamentId",
                table: "Matches");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamTournaments",
                table: "TeamTournaments",
                columns: new[] { "Id", "TeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamTournaments_TournamentId",
                table: "TeamTournaments",
                column: "TournamentId");
        }
    }
}
