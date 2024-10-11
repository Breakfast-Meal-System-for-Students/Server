using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v103_AddColumnQRCodeAndAddTableCartGroupUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "IsPurchase",
                table: "Carts",
                newName: "IsGroup");

            migrationBuilder.AddColumn<byte[]>(
                name: "QRCode",
                table: "Orders",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<bool>(
                name: "isPercentDiscount",
                table: "Coupons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CartGroupUserId",
                table: "CartDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CartGroupUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartGroupUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartGroupUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartGroupUser_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("09792857-b087-4f94-ba90-adbcfd9ef102"), null, "Admin", "Admin" },
                    { new Guid("4bd3773e-879a-458c-9254-61013dccb0cf"), null, "Staff", "Staff" },
                    { new Guid("745545a3-c25a-40b5-b0e5-70a340e9e3a5"), null, "Shop", "Shop" },
                    { new Guid("b72ad349-1082-404b-a76c-b0a49d709c60"), null, "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("2295e58c-a935-4534-bc51-88d27d381039"), new DateTime(2024, 10, 1, 13, 40, 36, 867, DateTimeKind.Local).AddTicks(5434), null, "SuShi", null, false, new DateTime(2024, 10, 1, 13, 40, 36, 867, DateTimeKind.Local).AddTicks(5435), "SuShi" },
                    { new Guid("a7eb28ea-f02c-4300-88ac-a276c21abbd1"), new DateTime(2024, 10, 1, 13, 40, 36, 867, DateTimeKind.Local).AddTicks(5417), null, "Rice", null, false, new DateTime(2024, 10, 1, 13, 40, 36, 867, DateTimeKind.Local).AddTicks(5430), "Rice" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_CartGroupUserId",
                table: "CartDetails",
                column: "CartGroupUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartGroupUser_CartId",
                table: "CartGroupUser",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartGroupUser_UserId",
                table: "CartGroupUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_CartGroupUser_CartGroupUserId",
                table: "CartDetails",
                column: "CartGroupUserId",
                principalTable: "CartGroupUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_CartGroupUser_CartGroupUserId",
                table: "CartDetails");

            migrationBuilder.DropTable(
                name: "CartGroupUser");

            migrationBuilder.DropIndex(
                name: "IX_CartDetails_CartGroupUserId",
                table: "CartDetails");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("09792857-b087-4f94-ba90-adbcfd9ef102"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4bd3773e-879a-458c-9254-61013dccb0cf"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("745545a3-c25a-40b5-b0e5-70a340e9e3a5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b72ad349-1082-404b-a76c-b0a49d709c60"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2295e58c-a935-4534-bc51-88d27d381039"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a7eb28ea-f02c-4300-88ac-a276c21abbd1"));

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "isPercentDiscount",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "CartGroupUserId",
                table: "CartDetails");

            migrationBuilder.RenameColumn(
                name: "IsGroup",
                table: "Carts",
                newName: "IsPurchase");

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
    }
}
