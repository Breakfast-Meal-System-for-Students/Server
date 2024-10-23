using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v107_AddTableShopWeeklyReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ShopWeeklyReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopWeeklyReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopWeeklyReports_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0013724d-f87e-4fca-a2fd-631f017b5ca4"), null, "Shop", "Shop" },
                    { new Guid("0d6398de-b26b-4ff7-9da4-4229f66ab0bc"), null, "Admin", "Admin" },
                    { new Guid("1d02915f-75c4-452d-ad96-1c2d5994d3ad"), null, "User", "User" },
                    { new Guid("74adf4f9-4b30-4d8a-bb10-e246ae652c7a"), null, "Staff", "Staff" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("32afa54c-8915-4bc1-b7e5-ab7183249895"), new DateTime(2024, 10, 11, 10, 13, 45, 577, DateTimeKind.Local).AddTicks(4935), null, "Rice", null, false, new DateTime(2024, 10, 11, 10, 13, 45, 577, DateTimeKind.Local).AddTicks(4947), "Rice" },
                    { new Guid("394aa975-db26-4742-81c1-9a3536e4e87c"), new DateTime(2024, 10, 11, 10, 13, 45, 577, DateTimeKind.Local).AddTicks(4951), null, "SuShi", null, false, new DateTime(2024, 10, 11, 10, 13, 45, 577, DateTimeKind.Local).AddTicks(4951), "SuShi" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopWeeklyReports_ShopId",
                table: "ShopWeeklyReports",
                column: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopWeeklyReports");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0013724d-f87e-4fca-a2fd-631f017b5ca4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0d6398de-b26b-4ff7-9da4-4229f66ab0bc"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1d02915f-75c4-452d-ad96-1c2d5994d3ad"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("74adf4f9-4b30-4d8a-bb10-e246ae652c7a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("32afa54c-8915-4bc1-b7e5-ab7183249895"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("394aa975-db26-4742-81c1-9a3536e4e87c"));

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
    }
}
