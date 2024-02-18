using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wmbaApp.Data.WMMigrations
{
    /// <inheritdoc />
    public partial class ugame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_competitorTeamId",
                table: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "competitorTeamId",
                table: "Games",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_competitorTeamId",
                table: "Games",
                column: "competitorTeamId",
                principalTable: "Teams",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_competitorTeamId",
                table: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "competitorTeamId",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_competitorTeamId",
                table: "Games",
                column: "competitorTeamId",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
