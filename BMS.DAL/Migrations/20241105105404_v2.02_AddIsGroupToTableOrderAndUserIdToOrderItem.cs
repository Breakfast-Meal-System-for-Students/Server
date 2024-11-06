using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v202_AddIsGroupToTableOrderAndUserIdToOrderItem : Migration
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("9e594aad-2be2-460b-a311-7dca774b3831"), null, "Admin", "Admin" },
                    { new Guid("a6212522-e9ec-4529-8c0a-24d021dbbc29"), null, "Staff", "Staff" },
                    { new Guid("b3e5157c-49d6-43be-830d-e6ca6e1b508a"), null, "User", "User" },
                    { new Guid("e46b93f7-92be-476e-ad6f-41f2e1f337a6"), null, "Shop", "Shop" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("8c5b6a83-bb4c-47e0-9a43-4ef3890b0b44"), new DateTime(2024, 11, 5, 17, 54, 3, 732, DateTimeKind.Local).AddTicks(7826), null, "SuShi", null, false, new DateTime(2024, 11, 5, 17, 54, 3, 732, DateTimeKind.Local).AddTicks(7827), "SuShi" },
                    { new Guid("ca4dca1b-5597-4a80-b7f9-935acddd6969"), new DateTime(2024, 11, 5, 17, 54, 3, 732, DateTimeKind.Local).AddTicks(7813), null, "Rice", null, false, new DateTime(2024, 11, 5, 17, 54, 3, 732, DateTimeKind.Local).AddTicks(7823), "Rice" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9e594aad-2be2-460b-a311-7dca774b3831"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a6212522-e9ec-4529-8c0a-24d021dbbc29"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b3e5157c-49d6-43be-830d-e6ca6e1b508a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e46b93f7-92be-476e-ad6f-41f2e1f337a6"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8c5b6a83-bb4c-47e0-9a43-4ef3890b0b44"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ca4dca1b-5597-4a80-b7f9-935acddd6969"));

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
