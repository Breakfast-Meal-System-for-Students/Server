using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v105_Addlocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<double>(
                name: "lat",
                table: "Shops",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "lng",
                table: "Shops",
                type: "float",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("04f31d03-e6e0-4972-912e-05be82633bc5"), null, "User", "User" },
                    { new Guid("1bf8fd24-edda-4fff-9382-8a25844666e8"), null, "Staff", "Staff" },
                    { new Guid("72f68a8b-3143-4b53-b74f-91fdc9833e3e"), null, "Admin", "Admin" },
                    { new Guid("d78ff065-7e33-4578-b81c-3ec88e35a99a"), null, "Shop", "Shop" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("0fd85273-e9b6-4875-ac49-3fe2c1aa736b"), new DateTime(2024, 10, 7, 22, 3, 43, 782, DateTimeKind.Local).AddTicks(6662), null, "Rice", null, false, new DateTime(2024, 10, 7, 22, 3, 43, 782, DateTimeKind.Local).AddTicks(6678), "Rice" },
                    { new Guid("49405edd-b343-4c47-9464-165ebed09761"), new DateTime(2024, 10, 7, 22, 3, 43, 782, DateTimeKind.Local).AddTicks(6685), null, "SuShi", null, false, new DateTime(2024, 10, 7, 22, 3, 43, 782, DateTimeKind.Local).AddTicks(6685), "SuShi" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("04f31d03-e6e0-4972-912e-05be82633bc5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1bf8fd24-edda-4fff-9382-8a25844666e8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("72f68a8b-3143-4b53-b74f-91fdc9833e3e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d78ff065-7e33-4578-b81c-3ec88e35a99a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0fd85273-e9b6-4875-ac49-3fe2c1aa736b"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("49405edd-b343-4c47-9464-165ebed09761"));

            migrationBuilder.DropColumn(
                name: "lat",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "lng",
                table: "Shops");

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
    }
}
