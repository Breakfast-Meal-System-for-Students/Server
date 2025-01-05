using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addShopUniversity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Universities_UniversityId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Shops_UniversityId",
                table: "Shops");

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
                name: "UniversityId",
                table: "Shops");

            migrationBuilder.CreateTable(
                name: "ShopUniversity",
                columns: table => new
                {
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniversityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopUniversity", x => new { x.ShopId, x.UniversityId });
                    table.ForeignKey(
                        name: "FK_ShopUniversity_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopUniversity_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("10d28717-8aec-4390-a1d8-dec487be3d00"), null, "Staff", "Staff" },
                    { new Guid("637bed99-73e3-469a-aae0-ab05c8b7e58f"), null, "Admin", "Admin" },
                    { new Guid("6bbb2589-a16c-4a0b-93b5-da20e9ccd607"), null, "Shop", "Shop" },
                    { new Guid("9b499627-35ce-421e-be74-3147680fada8"), null, "User", "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopUniversity_UniversityId",
                table: "ShopUniversity",
                column: "UniversityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopUniversity");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("10d28717-8aec-4390-a1d8-dec487be3d00"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("637bed99-73e3-469a-aae0-ab05c8b7e58f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6bbb2589-a16c-4a0b-93b5-da20e9ccd607"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9b499627-35ce-421e-be74-3147680fada8"));

            migrationBuilder.AddColumn<Guid>(
                name: "UniversityId",
                table: "Shops",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Shops_UniversityId",
                table: "Shops",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Universities_UniversityId",
                table: "Shops",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
