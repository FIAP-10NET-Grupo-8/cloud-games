using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.CloudGames.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserGameLibraryMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Library_Games_GameId",
                table: "Library");

            migrationBuilder.DropForeignKey(
                name: "FK_Library_Users_UserId",
                table: "Library");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Library",
                table: "Library");

            migrationBuilder.RenameTable(
                name: "Library",
                newName: "UserGameLibrary");

            migrationBuilder.RenameIndex(
                name: "IX_Library_GameId",
                table: "UserGameLibrary",
                newName: "IX_UserGameLibrary_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGameLibrary",
                table: "UserGameLibrary",
                columns: new[] { "UserId", "GameId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserGameLibrary_PurchaseDate",
                table: "UserGameLibrary",
                column: "PurchaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserGameLibrary_UserId",
                table: "UserGameLibrary",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGameLibrary_Games_GameId",
                table: "UserGameLibrary",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGameLibrary_Users_UserId",
                table: "UserGameLibrary",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGameLibrary_Games_GameId",
                table: "UserGameLibrary");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGameLibrary_Users_UserId",
                table: "UserGameLibrary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGameLibrary",
                table: "UserGameLibrary");

            migrationBuilder.DropIndex(
                name: "IX_UserGameLibrary_PurchaseDate",
                table: "UserGameLibrary");

            migrationBuilder.DropIndex(
                name: "IX_UserGameLibrary_UserId",
                table: "UserGameLibrary");

            migrationBuilder.RenameTable(
                name: "UserGameLibrary",
                newName: "Library");

            migrationBuilder.RenameIndex(
                name: "IX_UserGameLibrary_GameId",
                table: "Library",
                newName: "IX_Library_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Library",
                table: "Library",
                columns: new[] { "UserId", "GameId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Library_Games_GameId",
                table: "Library",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Library_Users_UserId",
                table: "Library",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
