﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISO810_ComprasProject.Migrations
{
    /// <inheritdoc />
    public partial class AnotherMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "proveedorId",
                table: "Proveedor",
                newName: "ProveedorId");

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProveedorId",
                table: "Proveedor",
                newName: "proveedorId");

         
        }
    }
}
