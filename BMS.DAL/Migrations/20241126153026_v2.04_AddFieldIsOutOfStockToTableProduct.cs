using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v204_AddFieldIsOutOfStockToTableProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("32dada2b-c076-4829-a83b-e187df79d50e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("59742f97-541b-4f4a-ac53-7e9c1bbeee2c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a0493c0e-737c-4f59-a6fc-ea8f221fb528"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b255df24-bfb5-479b-af1f-c4bb2b97064b"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0e5bd0b5-a1ad-4a52-8b26-96918c7de30a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2484514b-bda6-41a4-bda1-fb07426c398c"));

            migrationBuilder.AddColumn<bool>(
                name: "isOutOfStock",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("911ec44f-e598-4a57-b74c-68ddcf08dd5a"), null, "Admin", "Admin" },
                    { new Guid("97e30a7e-fd06-48c8-96d3-51e9e9f3e3b7"), null, "Staff", "Staff" },
                    { new Guid("a739e9f6-e5a4-45e1-b2c9-b156f436abbe"), null, "Shop", "Shop" },
                    { new Guid("b3449cee-e809-49ec-8145-ece229d05f50"), null, "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("2a79104a-caa4-4d13-bc80-6d20ee82572f"), new DateTime(2024, 11, 26, 22, 30, 26, 261, DateTimeKind.Local).AddTicks(5721), null, "SuShi", null, false, new DateTime(2024, 11, 26, 22, 30, 26, 261, DateTimeKind.Local).AddTicks(5722), "SuShi" },
                    { new Guid("4c5593fe-5090-4be7-9870-1ff9bfb858f4"), new DateTime(2024, 11, 26, 22, 30, 26, 261, DateTimeKind.Local).AddTicks(5707), null, "Rice", null, false, new DateTime(2024, 11, 26, 22, 30, 26, 261, DateTimeKind.Local).AddTicks(5716), "Rice" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("911ec44f-e598-4a57-b74c-68ddcf08dd5a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("97e30a7e-fd06-48c8-96d3-51e9e9f3e3b7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a739e9f6-e5a4-45e1-b2c9-b156f436abbe"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b3449cee-e809-49ec-8145-ece229d05f50"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2a79104a-caa4-4d13-bc80-6d20ee82572f"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4c5593fe-5090-4be7-9870-1ff9bfb858f4"));

            migrationBuilder.DropColumn(
                name: "isOutOfStock",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("32dada2b-c076-4829-a83b-e187df79d50e"), null, "Admin", "Admin" },
                    { new Guid("59742f97-541b-4f4a-ac53-7e9c1bbeee2c"), null, "Shop", "Shop" },
                    { new Guid("a0493c0e-737c-4f59-a6fc-ea8f221fb528"), null, "User", "User" },
                    { new Guid("b255df24-bfb5-479b-af1f-c4bb2b97064b"), null, "Staff", "Staff" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("0e5bd0b5-a1ad-4a52-8b26-96918c7de30a"), new DateTime(2024, 11, 22, 19, 23, 31, 174, DateTimeKind.Local).AddTicks(5714), null, "SuShi", null, false, new DateTime(2024, 11, 22, 19, 23, 31, 174, DateTimeKind.Local).AddTicks(5715), "SuShi" },
                    { new Guid("2484514b-bda6-41a4-bda1-fb07426c398c"), new DateTime(2024, 11, 22, 19, 23, 31, 174, DateTimeKind.Local).AddTicks(5700), null, "Rice", null, false, new DateTime(2024, 11, 22, 19, 23, 31, 174, DateTimeKind.Local).AddTicks(5710), "Rice" }
                });
        }
    }
}
