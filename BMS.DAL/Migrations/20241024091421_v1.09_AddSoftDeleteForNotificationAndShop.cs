using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v109_AddSoftDeleteForNotificationAndShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("20f0dacb-4a29-4819-82ab-1e011bd7b3e7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("25e96c02-f1f9-424e-835b-e2b69de69188"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a02e0b2b-a82b-4b80-940e-26d37b175cef"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b1bf0e23-05d5-43bb-9812-cc78568b2538"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3b6e6e51-2075-48df-b7d8-337df3fe6891"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("61a42337-0955-4d40-aa6a-533e6ab3ae34"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Shops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Shops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("14dd599d-03cf-43f8-ab6b-08bd94aaa401"), null, "Shop", "Shop" },
                    { new Guid("63c2b53d-4e79-4321-9d28-428ea2f1b3b6"), null, "Admin", "Admin" },
                    { new Guid("aabdae6a-6929-4e4b-977f-a32870a5d590"), null, "User", "User" },
                    { new Guid("fef2adca-af73-431b-87ad-5b24959786e9"), null, "Staff", "Staff" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("5dd756b7-8fef-4b7a-89b4-fa93ae29f1b1"), new DateTime(2024, 10, 24, 16, 14, 21, 312, DateTimeKind.Local).AddTicks(826), null, "SuShi", null, false, new DateTime(2024, 10, 24, 16, 14, 21, 312, DateTimeKind.Local).AddTicks(826), "SuShi" },
                    { new Guid("b5a0a232-83bb-48f2-939f-ad7b21cfba78"), new DateTime(2024, 10, 24, 16, 14, 21, 312, DateTimeKind.Local).AddTicks(812), null, "Rice", null, false, new DateTime(2024, 10, 24, 16, 14, 21, 312, DateTimeKind.Local).AddTicks(822), "Rice" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("14dd599d-03cf-43f8-ab6b-08bd94aaa401"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("63c2b53d-4e79-4321-9d28-428ea2f1b3b6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aabdae6a-6929-4e4b-977f-a32870a5d590"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("fef2adca-af73-431b-87ad-5b24959786e9"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5dd756b7-8fef-4b7a-89b4-fa93ae29f1b1"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b5a0a232-83bb-48f2-939f-ad7b21cfba78"));

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Notifications");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("20f0dacb-4a29-4819-82ab-1e011bd7b3e7"), null, "User", "User" },
                    { new Guid("25e96c02-f1f9-424e-835b-e2b69de69188"), null, "Admin", "Admin" },
                    { new Guid("a02e0b2b-a82b-4b80-940e-26d37b175cef"), null, "Shop", "Shop" },
                    { new Guid("b1bf0e23-05d5-43bb-9812-cc78568b2538"), null, "Staff", "Staff" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("3b6e6e51-2075-48df-b7d8-337df3fe6891"), new DateTime(2024, 10, 15, 10, 48, 19, 508, DateTimeKind.Local).AddTicks(9049), null, "SuShi", null, false, new DateTime(2024, 10, 15, 10, 48, 19, 508, DateTimeKind.Local).AddTicks(9050), "SuShi" },
                    { new Guid("61a42337-0955-4d40-aa6a-533e6ab3ae34"), new DateTime(2024, 10, 15, 10, 48, 19, 508, DateTimeKind.Local).AddTicks(9031), null, "Rice", null, false, new DateTime(2024, 10, 15, 10, 48, 19, 508, DateTimeKind.Local).AddTicks(9042), "Rice" }
                });
        }
    }
}
