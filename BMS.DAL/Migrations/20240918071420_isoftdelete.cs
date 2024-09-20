using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class isoftdelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("61f8f8ff-ade0-4027-b485-002f4da309d8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a26afa1f-e489-4fe3-95d6-1d519b63eb3a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a97e9132-f060-4e2c-9e61-8088afab5ff2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ae05af39-23fc-4bd9-8255-a6f755ffcdf3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5974dddc-3920-419a-9606-211ae25bc6a5"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ab41163c-2848-47a6-a11d-34b5e70b9086"));

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Products",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Packages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Packages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Coupons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Coupons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("38e369b2-1402-4760-9172-fe0647018689"), null, "Staff", "Staff" },
                    { new Guid("b7f3e764-b654-4ede-9eb4-2a0e0ed12c39"), null, "Shop", "Shop" },
                    { new Guid("d7168551-4367-490a-b429-021122fad471"), null, "User", "User" },
                    { new Guid("d7e8d45c-fd4b-45c8-82f4-0c9e341e59ed"), null, "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("49448aab-ec5b-44b5-a5c8-fbadf320e7e7"), new DateTime(2024, 9, 18, 14, 14, 20, 404, DateTimeKind.Local).AddTicks(4890), null, "Rice", null, false, new DateTime(2024, 9, 18, 14, 14, 20, 404, DateTimeKind.Local).AddTicks(4901), "Rice" },
                    { new Guid("806c86f3-61de-4b54-8d58-755ebd214f8a"), new DateTime(2024, 9, 18, 14, 14, 20, 404, DateTimeKind.Local).AddTicks(4905), null, "SuShi", null, false, new DateTime(2024, 9, 18, 14, 14, 20, 404, DateTimeKind.Local).AddTicks(4906), "SuShi" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("38e369b2-1402-4760-9172-fe0647018689"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b7f3e764-b654-4ede-9eb4-2a0e0ed12c39"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d7168551-4367-490a-b429-021122fad471"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d7e8d45c-fd4b-45c8-82f4-0c9e341e59ed"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("49448aab-ec5b-44b5-a5c8-fbadf320e7e7"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("806c86f3-61de-4b54-8d58-755ebd214f8a"));

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Products",
                newName: "Status");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("61f8f8ff-ade0-4027-b485-002f4da309d8"), null, "Admin", "Admin" },
                    { new Guid("a26afa1f-e489-4fe3-95d6-1d519b63eb3a"), null, "User", "User" },
                    { new Guid("a97e9132-f060-4e2c-9e61-8088afab5ff2"), null, "Staff", "Staff" },
                    { new Guid("ae05af39-23fc-4bd9-8255-a6f755ffcdf3"), null, "Shop", "Shop" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "Description", "Image", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("5974dddc-3920-419a-9606-211ae25bc6a5"), new DateTime(2024, 9, 12, 16, 27, 55, 27, DateTimeKind.Local).AddTicks(4959), "Rice", null, new DateTime(2024, 9, 12, 16, 27, 55, 27, DateTimeKind.Local).AddTicks(4973), "Rice" },
                    { new Guid("ab41163c-2848-47a6-a11d-34b5e70b9086"), new DateTime(2024, 9, 12, 16, 27, 55, 27, DateTimeKind.Local).AddTicks(4979), "SuShi", null, new DateTime(2024, 9, 12, 16, 27, 55, 27, DateTimeKind.Local).AddTicks(4979), "SuShi" }
                });
        }
    }
}
