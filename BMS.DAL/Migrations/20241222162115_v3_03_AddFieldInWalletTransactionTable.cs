using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v3_03_AddFieldInWalletTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("73763b56-92a1-41f3-aaa9-fe6c8dad745c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("75c5e41d-38c8-40f6-8e97-f1c661b5631a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9691b818-23ab-48bf-9deb-a965c55711c6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a56093b9-12d2-483e-90f6-0d413e910d69"));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "WalletTransactions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "WalletTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("163376a0-fc1f-471d-99d3-a411c14644e3"), null, "Staff", "Staff" },
                    { new Guid("556f8f82-538c-4be0-89bc-a21b67f48889"), null, "User", "User" },
                    { new Guid("82473362-1dc1-4b15-89e7-630dca5c0211"), null, "Admin", "Admin" },
                    { new Guid("ee7ffc48-0bb4-45de-8485-82144da5461d"), null, "Shop", "Shop" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("163376a0-fc1f-471d-99d3-a411c14644e3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("556f8f82-538c-4be0-89bc-a21b67f48889"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("82473362-1dc1-4b15-89e7-630dca5c0211"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee7ffc48-0bb4-45de-8485-82144da5461d"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WalletTransactions");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "WalletTransactions",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("73763b56-92a1-41f3-aaa9-fe6c8dad745c"), null, "Staff", "Staff" },
                    { new Guid("75c5e41d-38c8-40f6-8e97-f1c661b5631a"), null, "Shop", "Shop" },
                    { new Guid("9691b818-23ab-48bf-9deb-a965c55711c6"), null, "User", "User" },
                    { new Guid("a56093b9-12d2-483e-90f6-0d413e910d69"), null, "Admin", "Admin" }
                });
        }
    }
}
