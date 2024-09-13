using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryShops");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("22027808-8c69-4b07-8c93-80339b1a12a7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("60672cf0-51c2-4af9-b2c4-269bf6283744"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("63b2c868-c5e9-49d6-9a67-7098622d6b44"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f66c5d34-768b-4726-8b29-9ceb8a75c878"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("22a85b6e-4d76-479c-b077-76481815d1b8"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f51d20d8-0a9a-4e7d-9dd4-c67fe636c31f"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RegisterCategorys",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterCategorys", x => new { x.CategoryId, x.ShopId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_RegisterCategorys_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegisterCategorys_Products_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegisterCategorys_Shops_ShopId",
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

            migrationBuilder.CreateIndex(
                name: "IX_RegisterCategorys_ShopId",
                table: "RegisterCategorys",
                column: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisterCategorys");

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

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Feedbacks");

            migrationBuilder.CreateTable(
                name: "CategoryShops",
                columns: table => new
                {
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryShops", x => new { x.ShopId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_CategoryShops_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryShops_Shops_ShopId",
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
                    { new Guid("22027808-8c69-4b07-8c93-80339b1a12a7"), null, "Staff", "Staff" },
                    { new Guid("60672cf0-51c2-4af9-b2c4-269bf6283744"), null, "User", "User" },
                    { new Guid("63b2c868-c5e9-49d6-9a67-7098622d6b44"), null, "Shop", "Shop" },
                    { new Guid("f66c5d34-768b-4726-8b29-9ceb8a75c878"), null, "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "Description", "Image", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("22a85b6e-4d76-479c-b077-76481815d1b8"), new DateTime(2024, 9, 11, 14, 43, 29, 530, DateTimeKind.Local).AddTicks(3242), "SuShi", null, new DateTime(2024, 9, 11, 14, 43, 29, 530, DateTimeKind.Local).AddTicks(3242), "SuShi" },
                    { new Guid("f51d20d8-0a9a-4e7d-9dd4-c67fe636c31f"), new DateTime(2024, 9, 11, 14, 43, 29, 530, DateTimeKind.Local).AddTicks(3207), "Rice", null, new DateTime(2024, 9, 11, 14, 43, 29, 530, DateTimeKind.Local).AddTicks(3225), "Rice" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryShops_CategoryId",
                table: "CategoryShops",
                column: "CategoryId");
        }
    }
}
