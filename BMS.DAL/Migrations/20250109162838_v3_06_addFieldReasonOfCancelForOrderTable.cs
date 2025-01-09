using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v3_06_addFieldReasonOfCancelForOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "ReasonOfCancel",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("02790702-1edd-43b7-9346-f11adead0cd7"), null, "User", "User" },
                    { new Guid("3f5c2530-fdf1-4551-935a-6e57a5bf1a7c"), null, "Staff", "Staff" },
                    { new Guid("4d0de088-14b5-4304-9a16-ceefbe17390a"), null, "Admin", "Admin" },
                    { new Guid("87f032d0-77c4-42e9-bcfb-4eb9eee3c94d"), null, "Shop", "Shop" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("02790702-1edd-43b7-9346-f11adead0cd7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3f5c2530-fdf1-4551-935a-6e57a5bf1a7c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4d0de088-14b5-4304-9a16-ceefbe17390a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("87f032d0-77c4-42e9-bcfb-4eb9eee3c94d"));

            migrationBuilder.DropColumn(
                name: "ReasonOfCancel",
                table: "Orders");

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
    }
}
