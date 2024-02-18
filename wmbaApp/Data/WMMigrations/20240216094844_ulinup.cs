using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wmbaApp.Data.WMMigrations
{
    /// <inheritdoc />
    public partial class ulinup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PlayersLineUp");

            migrationBuilder.AddColumn<int>(
                name: "CompetitorTeamId",
                table: "PlayersLineUp",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayersLineUp_CompetitorTeamId",
                table: "PlayersLineUp",
                column: "CompetitorTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersLineUp_Teams_CompetitorTeamId",
                table: "PlayersLineUp",
                column: "CompetitorTeamId",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayersLineUp_Teams_CompetitorTeamId",
                table: "PlayersLineUp");

            migrationBuilder.DropIndex(
                name: "IX_PlayersLineUp_CompetitorTeamId",
                table: "PlayersLineUp");

            migrationBuilder.DropColumn(
                name: "CompetitorTeamId",
                table: "PlayersLineUp");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PlayersLineUp",
                type: "TEXT",
                nullable: true);
        }
    }
}
