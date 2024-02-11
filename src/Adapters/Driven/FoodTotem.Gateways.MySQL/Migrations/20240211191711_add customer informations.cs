using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodTotem.Gateways.MySQL.Migrations
{
    /// <inheritdoc />
    public partial class addcustomerinformations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "identity",
                table: "Customer",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "identity",
                table: "Customer",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                schema: "identity",
                table: "Customer",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "identity",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "identity",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "identity",
                table: "Customer");
        }
    }
}
