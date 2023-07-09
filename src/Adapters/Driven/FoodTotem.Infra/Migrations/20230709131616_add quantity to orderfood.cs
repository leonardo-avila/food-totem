using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodTotem.Infra.Migrations
{
    /// <inheritdoc />
    public partial class addquantitytoorderfood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OrderFood",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderFood");
        }
    }
}
