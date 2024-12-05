using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v205_AddFeildIsAICanDetectToTableProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "isAICanDetect",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0a1e8da0-c37e-4226-ac2f-b99015c86618"), null, "Admin", "Admin" },
                    { new Guid("493846dd-439a-4254-bf54-a7cc992b8c3f"), null, "User", "User" },
                    { new Guid("949034d9-df9c-4d52-affd-f2953f50f976"), null, "Staff", "Staff" },
                    { new Guid("ed9cb057-35b0-410c-8a86-54dc3bed8c49"), null, "Shop", "Shop" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("1be263c2-119f-4c91-b8a9-82d9cefc4d85"), new DateTime(2024, 12, 5, 12, 9, 7, 74, DateTimeKind.Utc).AddTicks(7720), null, "Rice", null, false, new DateTime(2024, 12, 5, 12, 9, 7, 74, DateTimeKind.Utc).AddTicks(7731), "Rice" },
                    { new Guid("74fecdcd-6699-4d38-9209-ee59f1810255"), new DateTime(2024, 12, 5, 12, 9, 7, 74, DateTimeKind.Utc).AddTicks(7742), null, "SuShi", null, false, new DateTime(2024, 12, 5, 12, 9, 7, 74, DateTimeKind.Utc).AddTicks(7743), "SuShi" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0a1e8da0-c37e-4226-ac2f-b99015c86618"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("493846dd-439a-4254-bf54-a7cc992b8c3f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("949034d9-df9c-4d52-affd-f2953f50f976"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ed9cb057-35b0-410c-8a86-54dc3bed8c49"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("1be263c2-119f-4c91-b8a9-82d9cefc4d85"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("74fecdcd-6699-4d38-9209-ee59f1810255"));

            migrationBuilder.DropColumn(
                name: "isAICanDetect",
                table: "Products");

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
    }
}
