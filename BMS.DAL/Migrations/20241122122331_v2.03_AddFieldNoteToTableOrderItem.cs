using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v203_AddFieldNoteToTableOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("18086028-9bde-49d5-8fd0-d5ae95540136"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("20c108ff-44d2-4788-a365-24b71440b408"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cf01d670-dfc1-4690-abb5-115e555dd2ba"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e49ed277-786a-442c-90ba-c82828e301f2"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("41cd0bee-df8b-4c20-99ae-719e2457495c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ae177975-1141-4588-83d9-1ac64eb9dcbe"));

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Note",
                table: "OrderItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("18086028-9bde-49d5-8fd0-d5ae95540136"), null, "User", "User" },
                    { new Guid("20c108ff-44d2-4788-a365-24b71440b408"), null, "Shop", "Shop" },
                    { new Guid("cf01d670-dfc1-4690-abb5-115e555dd2ba"), null, "Staff", "Staff" },
                    { new Guid("e49ed277-786a-442c-90ba-c82828e301f2"), null, "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("41cd0bee-df8b-4c20-99ae-719e2457495c"), new DateTime(2024, 11, 15, 20, 59, 1, 755, DateTimeKind.Local).AddTicks(949), null, "SuShi", null, false, new DateTime(2024, 11, 15, 20, 59, 1, 755, DateTimeKind.Local).AddTicks(949), "SuShi" },
                    { new Guid("ae177975-1141-4588-83d9-1ac64eb9dcbe"), new DateTime(2024, 11, 15, 20, 59, 1, 755, DateTimeKind.Local).AddTicks(935), null, "Rice", null, false, new DateTime(2024, 11, 15, 20, 59, 1, 755, DateTimeKind.Local).AddTicks(945), "Rice" }
                });
        }
    }
}
