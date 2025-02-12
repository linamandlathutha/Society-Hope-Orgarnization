using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocietyHopeOrg.Data.Migrations
{
    /// <inheritdoc />
    public partial class Tracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackingCode",
                table: "ToyDonations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingCode",
                table: "GroceryDonations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingCode",
                table: "ClothingDonations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingCode",
                table: "ToyDonations");

            migrationBuilder.DropColumn(
                name: "TrackingCode",
                table: "GroceryDonations");

            migrationBuilder.DropColumn(
                name: "TrackingCode",
                table: "ClothingDonations");
        }
    }
}
