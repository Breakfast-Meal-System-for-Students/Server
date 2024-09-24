using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v102_AddColumnPriceInOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("19cedbee-6b0e-4056-ae33-cdefb87d6eba"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1d2159f5-e412-4714-8fcc-15ee5a90681a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("35077bd5-1787-40ea-94cd-b25328ce8ad5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f48cddb7-b377-4e30-a8ea-077651bc0642"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6f148eac-b9fa-4e9b-bf80-386e00294a28"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a9f8cd9e-969f-4c43-acc1-9bbb58d74c4a"));

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "OrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0e26662a-da7a-46de-af43-3402eaa9fb81"), null, "User", "User" },
                    { new Guid("75b6ef7a-f03d-4f08-bf77-cbae20362528"), null, "Admin", "Admin" },
                    { new Guid("c8e7f53e-9cf1-4ffa-b4c6-55903a73e42f"), null, "Shop", "Shop" },
                    { new Guid("f5041366-9906-4a25-baf4-78f3d1c4ab41"), null, "Staff", "Staff" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("340f61b6-994d-426a-a037-5fc9c93d4d56"), new DateTime(2024, 9, 25, 0, 12, 1, 382, DateTimeKind.Local).AddTicks(4833), null, "SuShi", null, false, new DateTime(2024, 9, 25, 0, 12, 1, 382, DateTimeKind.Local).AddTicks(4833), "SuShi" },
                    { new Guid("f645678b-fa73-4f3a-b859-dfc16eced1a9"), new DateTime(2024, 9, 25, 0, 12, 1, 382, DateTimeKind.Local).AddTicks(4819), null, "Rice", null, false, new DateTime(2024, 9, 25, 0, 12, 1, 382, DateTimeKind.Local).AddTicks(4829), "Rice" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0e26662a-da7a-46de-af43-3402eaa9fb81"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("75b6ef7a-f03d-4f08-bf77-cbae20362528"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c8e7f53e-9cf1-4ffa-b4c6-55903a73e42f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f5041366-9906-4a25-baf4-78f3d1c4ab41"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("340f61b6-994d-426a-a037-5fc9c93d4d56"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f645678b-fa73-4f3a-b859-dfc16eced1a9"));

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("19cedbee-6b0e-4056-ae33-cdefb87d6eba"), null, "Shop", "Shop" },
                    { new Guid("1d2159f5-e412-4714-8fcc-15ee5a90681a"), null, "Staff", "Staff" },
                    { new Guid("35077bd5-1787-40ea-94cd-b25328ce8ad5"), null, "Admin", "Admin" },
                    { new Guid("f48cddb7-b377-4e30-a8ea-077651bc0642"), null, "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("6f148eac-b9fa-4e9b-bf80-386e00294a28"), new DateTime(2024, 9, 21, 21, 17, 40, 513, DateTimeKind.Local).AddTicks(3983), null, "SuShi", null, false, new DateTime(2024, 9, 21, 21, 17, 40, 513, DateTimeKind.Local).AddTicks(3983), "SuShi" },
                    { new Guid("a9f8cd9e-969f-4c43-acc1-9bbb58d74c4a"), new DateTime(2024, 9, 21, 21, 17, 40, 513, DateTimeKind.Local).AddTicks(3934), null, "Rice", null, false, new DateTime(2024, 9, 21, 21, 17, 40, 513, DateTimeKind.Local).AddTicks(3946), "Rice" }
                });
        }
    }
}
