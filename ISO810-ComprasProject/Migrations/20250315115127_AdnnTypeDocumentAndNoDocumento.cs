using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISO810_ComprasProject.Migrations
{
    /// <inheritdoc />
    public partial class AdnnTypeDocumentAndNoDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeroDocumento",
                table: "Proveedor",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoDocumento",
                table: "Proveedor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroDocumento",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "TipoDocumento",
                table: "Proveedor");
        }
    }
}
