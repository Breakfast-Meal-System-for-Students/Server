using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v202_AddOTPTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<bool>(
                name: "IsGroup",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OTPs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Otp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OTPs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_OTPs_UserId",
                table: "OTPs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OTPs");

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

            migrationBuilder.DropColumn(
                name: "IsGroup",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OrderItems");

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
        }
    }
}
