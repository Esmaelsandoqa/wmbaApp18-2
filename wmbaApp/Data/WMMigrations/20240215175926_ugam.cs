using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wmbaApp.Data.WMMigrations
{
    /// <inheritdoc />
    public partial class ugam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "competitorTeam",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "team",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "competitorTeamId",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "teamId",
                table: "Games",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_competitorTeamId",
                table: "Games",
                column: "competitorTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_teamId",
                table: "Games",
                column: "teamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_competitorTeamId",
                table: "Games",
                column: "competitorTeamId",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_teamId",
                table: "Games",
                column: "teamId",
                principalTable: "Teams",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_competitorTeamId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_teamId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_competitorTeamId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_teamId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "competitorTeamId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "teamId",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "competitorTeam",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "team",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
