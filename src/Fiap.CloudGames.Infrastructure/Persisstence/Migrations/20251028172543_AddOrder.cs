using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.CloudGames.Infrastructure.Persisstence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalValue = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RefundRequested = table.Column<bool>(type: "INTEGER", nullable: false),
                    RefundReason = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    RefundRequestDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RefundDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PaymentTransactionId = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    PaymentConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
