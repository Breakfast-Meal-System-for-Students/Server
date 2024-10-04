using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v104_AddColumnTitleInNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("09792857-b087-4f94-ba90-adbcfd9ef102"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4bd3773e-879a-458c-9254-61013dccb0cf"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("745545a3-c25a-40b5-b0e5-70a340e9e3a5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b72ad349-1082-404b-a76c-b0a49d709c60"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2295e58c-a935-4534-bc51-88d27d381039"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a7eb28ea-f02c-4300-88ac-a276c21abbd1"));

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Notifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Title",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "CartDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("23edfe7e-1c66-4368-896d-a720218ac848"), null, "Staff", "Staff" },
                    { new Guid("91a62291-da65-4ff9-9c06-03119dfd03e9"), null, "User", "User" },
                    { new Guid("99672298-7145-48be-9e80-491febf0659c"), null, "Shop", "Shop" },
                    { new Guid("bdfda30b-64f4-4f07-8e88-7108cd3f2bd2"), null, "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("85ed8786-2291-4cfb-a890-6f2c4eff9f2f"), new DateTime(2024, 10, 3, 15, 53, 33, 354, DateTimeKind.Local).AddTicks(9554), null, "SuShi", null, false, new DateTime(2024, 10, 3, 15, 53, 33, 354, DateTimeKind.Local).AddTicks(9554), "SuShi" },
                    { new Guid("ceeacc54-909f-4de4-ac2c-f029182df9a3"), new DateTime(2024, 10, 3, 15, 53, 33, 354, DateTimeKind.Local).AddTicks(9537), null, "Rice", null, false, new DateTime(2024, 10, 3, 15, 53, 33, 354, DateTimeKind.Local).AddTicks(9551), "Rice" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("23edfe7e-1c66-4368-896d-a720218ac848"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("91a62291-da65-4ff9-9c06-03119dfd03e9"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("99672298-7145-48be-9e80-491febf0659c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bdfda30b-64f4-4f07-8e88-7108cd3f2bd2"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("85ed8786-2291-4cfb-a890-6f2c4eff9f2f"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ceeacc54-909f-4de4-ac2c-f029182df9a3"));

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "CartDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("09792857-b087-4f94-ba90-adbcfd9ef102"), null, "Admin", "Admin" },
                    { new Guid("4bd3773e-879a-458c-9254-61013dccb0cf"), null, "Staff", "Staff" },
                    { new Guid("745545a3-c25a-40b5-b0e5-70a340e9e3a5"), null, "Shop", "Shop" },
                    { new Guid("b72ad349-1082-404b-a76c-b0a49d709c60"), null, "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("2295e58c-a935-4534-bc51-88d27d381039"), new DateTime(2024, 10, 1, 13, 40, 36, 867, DateTimeKind.Local).AddTicks(5434), null, "SuShi", null, false, new DateTime(2024, 10, 1, 13, 40, 36, 867, DateTimeKind.Local).AddTicks(5435), "SuShi" },
                    { new Guid("a7eb28ea-f02c-4300-88ac-a276c21abbd1"), new DateTime(2024, 10, 1, 13, 40, 36, 867, DateTimeKind.Local).AddTicks(5417), null, "Rice", null, false, new DateTime(2024, 10, 1, 13, 40, 36, 867, DateTimeKind.Local).AddTicks(5430), "Rice" }
                });
        }
    }
}
