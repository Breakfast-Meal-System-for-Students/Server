using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v3_05_addFieldForPackageShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("10d28717-8aec-4390-a1d8-dec487be3d00"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("637bed99-73e3-469a-aae0-ab05c8b7e58f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6bbb2589-a16c-4a0b-93b5-da20e9ccd607"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9b499627-35ce-421e-be74-3147680fada8"));

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Package_Shops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Package_Shops",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0230cbf5-37e8-4298-a771-28fdc3f54105"), null, "User", "User" },
                    { new Guid("11b550aa-0dd7-4617-8c06-835e1f6b8a63"), null, "Admin", "Admin" },
                    { new Guid("13dea8e1-c879-4407-b683-630dfbff0a1d"), null, "Shop", "Shop" },
                    { new Guid("51664f56-0bab-410f-a2e1-d0d4af7a8a16"), null, "Staff", "Staff" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0230cbf5-37e8-4298-a771-28fdc3f54105"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("11b550aa-0dd7-4617-8c06-835e1f6b8a63"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("13dea8e1-c879-4407-b683-630dfbff0a1d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("51664f56-0bab-410f-a2e1-d0d4af7a8a16"));

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Package_Shops");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Package_Shops");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("10d28717-8aec-4390-a1d8-dec487be3d00"), null, "Staff", "Staff" },
                    { new Guid("637bed99-73e3-469a-aae0-ab05c8b7e58f"), null, "Admin", "Admin" },
                    { new Guid("6bbb2589-a16c-4a0b-93b5-da20e9ccd607"), null, "Shop", "Shop" },
                    { new Guid("9b499627-35ce-421e-be74-3147680fada8"), null, "User", "User" }
                });
        }
    }
}
