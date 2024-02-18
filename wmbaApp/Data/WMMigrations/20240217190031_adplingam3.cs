using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wmbaApp.Data.WMMigrations
{
    /// <inheritdoc />
    public partial class adplingam3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_PlayersLineUp_PlayersLineUpId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_PlayersLineUpId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PlayersLineUpId",
                table: "Games");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayersLineUpId",
                table: "Games",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlayersLineUpId",
                table: "Games",
                column: "PlayersLineUpId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_PlayersLineUp_PlayersLineUpId",
                table: "Games",
                column: "PlayersLineUpId",
                principalTable: "PlayersLineUp",
                principalColumn: "Id");
        }
    }
}
