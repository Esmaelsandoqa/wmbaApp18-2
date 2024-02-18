using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wmbaApp.Data.WMMigrations
{
    /// <inheritdoc />
    public partial class ulinup3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_PlayersLineUp_playersLineUpId",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "playersLineUpId1",
                table: "Players",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_playersLineUpId1",
                table: "Players",
                column: "playersLineUpId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_PlayersLineUp_playersLineUpId",
                table: "Players",
                column: "playersLineUpId",
                principalTable: "PlayersLineUp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_PlayersLineUp_playersLineUpId1",
                table: "Players",
                column: "playersLineUpId1",
                principalTable: "PlayersLineUp",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_PlayersLineUp_playersLineUpId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_PlayersLineUp_playersLineUpId1",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_playersLineUpId1",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "playersLineUpId1",
                table: "Players");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_PlayersLineUp_playersLineUpId",
                table: "Players",
                column: "playersLineUpId",
                principalTable: "PlayersLineUp",
                principalColumn: "Id");
        }
    }
}
