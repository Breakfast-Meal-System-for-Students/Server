using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v201_AddTableFavourite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<Guid>(
                name: "FeedbackId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Feedbacks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Favourites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favourites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Favourites_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("02b4919f-9fcb-4cb9-9d8a-764d3d2792d2"), null, "Staff", "Staff" },
                    { new Guid("157292c2-f1c2-4694-a0fa-3f8b42811b44"), null, "Shop", "Shop" },
                    { new Guid("72309cc9-ec01-49c6-a0ba-bf514e783bef"), null, "User", "User" },
                    { new Guid("dbd49c95-ae62-4b9d-acf6-f11b7577cc82"), null, "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("b29de934-4fa8-4482-a56a-1b06e49ac022"), new DateTime(2024, 11, 1, 11, 54, 1, 492, DateTimeKind.Local).AddTicks(8032), null, "SuShi", null, false, new DateTime(2024, 11, 1, 11, 54, 1, 492, DateTimeKind.Local).AddTicks(8033), "SuShi" },
                    { new Guid("da6a39b9-c5e1-45ee-89e7-e44fb50d4c81"), new DateTime(2024, 11, 1, 11, 54, 1, 492, DateTimeKind.Local).AddTicks(8018), null, "Rice", null, false, new DateTime(2024, 11, 1, 11, 54, 1, 492, DateTimeKind.Local).AddTicks(8028), "Rice" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FeedbackId",
                table: "Orders",
                column: "FeedbackId",
                unique: true,
                filter: "[FeedbackId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_ShopId",
                table: "Favourites",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_UserId",
                table: "Favourites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Feedbacks_FeedbackId",
                table: "Orders",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Feedbacks_FeedbackId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Favourites");

            migrationBuilder.DropIndex(
                name: "IX_Orders_FeedbackId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("02b4919f-9fcb-4cb9-9d8a-764d3d2792d2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("157292c2-f1c2-4694-a0fa-3f8b42811b44"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("72309cc9-ec01-49c6-a0ba-bf514e783bef"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("dbd49c95-ae62-4b9d-acf6-f11b7577cc82"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b29de934-4fa8-4482-a56a-1b06e49ac022"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("da6a39b9-c5e1-45ee-89e7-e44fb50d4c81"));

            migrationBuilder.DropColumn(
                name: "FeedbackId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Feedbacks");

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
        }
    }
}
