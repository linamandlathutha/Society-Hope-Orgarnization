using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocietyHopeOrg.Data.Migrations
{
    /// <inheritdoc />
    public partial class HChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodUsageTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsageDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CannedFoodQuantityUsed = table.Column<int>(type: "int", nullable: false),
                    BeansQuantityUsed = table.Column<int>(type: "int", nullable: false),
                    PastaOrRiceQuantityUsed = table.Column<int>(type: "int", nullable: false),
                    SugarFlourSaltCondimentsQuantityUsed = table.Column<int>(type: "int", nullable: false),
                    BabyProductsQuantityUsed = table.Column<int>(type: "int", nullable: false),
                    AvailableCannedFoodQuantity = table.Column<int>(type: "int", nullable: false),
                    AvailableBeansQuantity = table.Column<int>(type: "int", nullable: false),
                    AvailablePastaOrRiceQuantity = table.Column<int>(type: "int", nullable: false),
                    AvailableSugarFlourSaltCondimentsQuantity = table.Column<int>(type: "int", nullable: false),
                    AvailableBabyProductsQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodUsageTransactions", x => x.TransactionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodUsageTransactions");
        }
    }
}
