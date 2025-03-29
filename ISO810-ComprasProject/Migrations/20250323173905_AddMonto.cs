using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISO810_ComprasProject.Migrations
{
    /// <inheritdoc />
    public partial class AddMonto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Monto",
                table: "OrdenCompra",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Monto",
                table: "OrdenCompra");
        }
    }
}
