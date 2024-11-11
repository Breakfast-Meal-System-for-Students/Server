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
                    { new Guid("29f9c13f-8e7b-46dc-8d71-642a61908fbd"), null, "Admin", "Admin" },
                    { new Guid("2a921e2c-d17e-40b9-8a29-2740036c2023"), null, "User", "User" },
                    { new Guid("5022ffc8-03d6-4a82-82c7-08e932a69add"), null, "Staff", "Staff" },
                    { new Guid("813afcf4-3389-43ff-ae31-a1fc0bb99725"), null, "Shop", "Shop" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("711ba5f0-4d47-434f-95a2-eb910a9679a3"), new DateTime(2024, 11, 11, 22, 0, 11, 202, DateTimeKind.Local).AddTicks(8805), null, "Rice", null, false, new DateTime(2024, 11, 11, 22, 0, 11, 202, DateTimeKind.Local).AddTicks(8891), "Rice" },
                    { new Guid("d6f8e1f4-a964-4845-b5ec-35ca95a335c8"), new DateTime(2024, 11, 11, 22, 0, 11, 202, DateTimeKind.Local).AddTicks(8896), null, "SuShi", null, false, new DateTime(2024, 11, 11, 22, 0, 11, 202, DateTimeKind.Local).AddTicks(8897), "SuShi" }
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
                keyValue: new Guid("29f9c13f-8e7b-46dc-8d71-642a61908fbd"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2a921e2c-d17e-40b9-8a29-2740036c2023"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5022ffc8-03d6-4a82-82c7-08e932a69add"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("813afcf4-3389-43ff-ae31-a1fc0bb99725"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("711ba5f0-4d47-434f-95a2-eb910a9679a3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d6f8e1f4-a964-4845-b5ec-35ca95a335c8"));

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
