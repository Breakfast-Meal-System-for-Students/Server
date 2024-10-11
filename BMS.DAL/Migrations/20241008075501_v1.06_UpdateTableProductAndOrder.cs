using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v106_UpdateTableProductAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "Inventory",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PrepareTime",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("14b6c865-c0bb-4eb1-a7ba-d7eaa1de1437"), null, "Admin", "Admin" },
                    { new Guid("77ae2c61-9efd-48e3-8d33-a5d20429893f"), null, "Shop", "Shop" },
                    { new Guid("916ba9d9-a77a-4ea2-9b4c-bbd923976c06"), null, "Staff", "Staff" },
                    { new Guid("e7c7b516-7090-445a-af0c-cf843d8c84d3"), null, "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("697d3a27-f1df-42f0-b6bf-a2df7b72a534"), new DateTime(2024, 10, 8, 14, 55, 0, 738, DateTimeKind.Local).AddTicks(8327), null, "SuShi", null, false, new DateTime(2024, 10, 8, 14, 55, 0, 738, DateTimeKind.Local).AddTicks(8327), "SuShi" },
                    { new Guid("8dbf0d31-d0b7-497b-bf63-ee334cbbc5a0"), new DateTime(2024, 10, 8, 14, 55, 0, 738, DateTimeKind.Local).AddTicks(8313), null, "Rice", null, false, new DateTime(2024, 10, 8, 14, 55, 0, 738, DateTimeKind.Local).AddTicks(8323), "Rice" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("14b6c865-c0bb-4eb1-a7ba-d7eaa1de1437"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("77ae2c61-9efd-48e3-8d33-a5d20429893f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("916ba9d9-a77a-4ea2-9b4c-bbd923976c06"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e7c7b516-7090-445a-af0c-cf843d8c84d3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("697d3a27-f1df-42f0-b6bf-a2df7b72a534"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8dbf0d31-d0b7-497b-bf63-ee334cbbc5a0"));

            migrationBuilder.DropColumn(
                name: "Inventory",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PrepareTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Orders");

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
    }
}
