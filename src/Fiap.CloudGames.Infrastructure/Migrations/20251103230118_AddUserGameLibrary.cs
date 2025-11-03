using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.CloudGames.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserGameLibrary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGameLibrary",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGameLibrary", x => new { x.UserId, x.GameId });
                    table.ForeignKey(
                        name: "FK_UserGameLibrary_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGameLibrary_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGameLibrary_GameId",
                table: "UserGameLibrary",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGameLibrary_PurchaseDate",
                table: "UserGameLibrary",
                column: "PurchaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserGameLibrary_UserId",
                table: "UserGameLibrary",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGameLibrary");
        }
    }
}
