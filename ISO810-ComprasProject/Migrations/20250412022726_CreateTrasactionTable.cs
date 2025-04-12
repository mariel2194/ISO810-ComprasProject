using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISO810_ComprasProject.Migrations
{
    /// <inheritdoc />
    public partial class CreateTrasactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransaccionId",
                table: "OrdenCompra",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Transaccion",
                columns: table => new
                {
                    TransaccionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AsientoId = table.Column<int>(type: "int", nullable: true),
                    FechaAsiento = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaccion", x => x.TransaccionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdenCompra_TransaccionId",
                table: "OrdenCompra",
                column: "TransaccionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenCompra_Transaccion_TransaccionId",
                table: "OrdenCompra",
                column: "TransaccionId",
                principalTable: "Transaccion",
                principalColumn: "TransaccionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdenCompra_Transaccion_TransaccionId",
                table: "OrdenCompra");

            migrationBuilder.DropTable(
                name: "Transaccion");

            migrationBuilder.DropIndex(
                name: "IX_OrdenCompra_TransaccionId",
                table: "OrdenCompra");

            migrationBuilder.DropColumn(
                name: "TransaccionId",
                table: "OrdenCompra");
        }
    }
}
