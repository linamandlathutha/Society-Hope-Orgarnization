using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocietyHopeOrg.Data.Migrations
{
    /// <inheritdoc />
    public partial class quantities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FoodType",
                table: "GroceryDonations",
                newName: "SugarFlourSaltCondimentsQuantity");

            migrationBuilder.AddColumn<int>(
                name: "BabyProductsQuantity",
                table: "GroceryDonations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BeansQuantity",
                table: "GroceryDonations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CannedFoodQuantity",
                table: "GroceryDonations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PastaOrRiceQuantity",
                table: "GroceryDonations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FoodInventories",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CannedFoods = table.Column<int>(type: "int", nullable: false),
                    Beans = table.Column<int>(type: "int", nullable: false),
                    PastaOrRice = table.Column<int>(type: "int", nullable: false),
                    SugarFlourSaltCondiments = table.Column<int>(type: "int", nullable: false),
                    BabyProducts = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodInventories", x => x.InventoryId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodInventories");

            migrationBuilder.DropColumn(
                name: "BabyProductsQuantity",
                table: "GroceryDonations");

            migrationBuilder.DropColumn(
                name: "BeansQuantity",
                table: "GroceryDonations");

            migrationBuilder.DropColumn(
                name: "CannedFoodQuantity",
                table: "GroceryDonations");

            migrationBuilder.DropColumn(
                name: "PastaOrRiceQuantity",
                table: "GroceryDonations");

            migrationBuilder.RenameColumn(
                name: "SugarFlourSaltCondimentsQuantity",
                table: "GroceryDonations",
                newName: "FoodType");
        }
    }
}
