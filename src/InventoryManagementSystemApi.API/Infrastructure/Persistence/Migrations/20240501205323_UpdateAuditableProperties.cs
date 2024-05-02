using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementSystemApi.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditableProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "public",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "public",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "public",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "public",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "DateLastModified",
                schema: "public",
                table: "Warehouses",
                newName: "DateLastModifiedUtc");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                schema: "public",
                table: "Warehouses",
                newName: "DateCreatedUtc");

            migrationBuilder.RenameColumn(
                name: "DateLastModified",
                schema: "public",
                table: "Products",
                newName: "DateLastModifiedUtc");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                schema: "public",
                table: "Products",
                newName: "DateCreatedUtc");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                schema: "public",
                table: "Warehouses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                schema: "public",
                table: "Warehouses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                schema: "public",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                schema: "public",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "public",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                schema: "public",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "public",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                schema: "public",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "DateLastModifiedUtc",
                schema: "public",
                table: "Warehouses",
                newName: "DateLastModified");

            migrationBuilder.RenameColumn(
                name: "DateCreatedUtc",
                schema: "public",
                table: "Warehouses",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "DateLastModifiedUtc",
                schema: "public",
                table: "Products",
                newName: "DateLastModified");

            migrationBuilder.RenameColumn(
                name: "DateCreatedUtc",
                schema: "public",
                table: "Products",
                newName: "DateCreated");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "public",
                table: "Warehouses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                schema: "public",
                table: "Warehouses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "public",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                schema: "public",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
