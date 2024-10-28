using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v110_AddNotificationDestinationColumnForNotificationTableAndUpdateTableNamePackageHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageHistories");

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

            migrationBuilder.AddColumn<int>(
                name: "Destination",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Package_Shops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package_Shops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Package_Shops_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Package_Shops_Shops_ShopId",
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
                    { new Guid("2b54a6a1-1671-4388-a88e-d9467adec25f"), null, "Shop", "Shop" },
                    { new Guid("32b90b0e-faaa-491f-a2f4-3f718a0a48f6"), null, "Staff", "Staff" },
                    { new Guid("751b07a9-c39e-4ef3-b14e-481a01ff04d4"), null, "Admin", "Admin" },
                    { new Guid("e079e7b1-df70-4382-9e53-26333ee009fa"), null, "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("0ee56921-5baf-403d-96ed-ee69159417fa"), new DateTime(2024, 10, 28, 20, 1, 53, 25, DateTimeKind.Local).AddTicks(4873), null, "SuShi", null, false, new DateTime(2024, 10, 28, 20, 1, 53, 25, DateTimeKind.Local).AddTicks(4873), "SuShi" },
                    { new Guid("ef7f3b35-6c73-4a87-9b3b-bca82ae097ed"), new DateTime(2024, 10, 28, 20, 1, 53, 25, DateTimeKind.Local).AddTicks(4858), null, "Rice", null, false, new DateTime(2024, 10, 28, 20, 1, 53, 25, DateTimeKind.Local).AddTicks(4869), "Rice" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Package_Shops_PackageId",
                table: "Package_Shops",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_Shops_ShopId",
                table: "Package_Shops",
                column: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Package_Shops");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2b54a6a1-1671-4388-a88e-d9467adec25f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("32b90b0e-faaa-491f-a2f4-3f718a0a48f6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("751b07a9-c39e-4ef3-b14e-481a01ff04d4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e079e7b1-df70-4382-9e53-26333ee009fa"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0ee56921-5baf-403d-96ed-ee69159417fa"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ef7f3b35-6c73-4a87-9b3b-bca82ae097ed"));

            migrationBuilder.DropColumn(
                name: "Destination",
                table: "Notifications");

            migrationBuilder.CreateTable(
                name: "PackageHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageHistories_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageHistories_Shops_ShopId",
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

            migrationBuilder.CreateIndex(
                name: "IX_PackageHistories_PackageId",
                table: "PackageHistories",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageHistories_ShopId",
                table: "PackageHistories",
                column: "ShopId");
        }
    }
}
