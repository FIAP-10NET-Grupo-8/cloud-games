using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.CloudGames.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Developer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Publisher = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Platforms = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ConfirmationToken = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    PasswordResetToken = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    PasswordResetExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FirstAccessToken = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    FirstAccessExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
