using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v103_AddColumnPriceInOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("515e9800-fc1b-4969-bfc6-12e1312e4c70"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8caf1fe3-6485-4a51-a182-17905d06fa41"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b6da9c71-11fa-4385-a880-43883a779805"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d65c7293-8646-4cdb-9949-2ffa014b6b64"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("435d4e27-87c1-4adc-b0de-972ac53304e3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ccb39ba3-340e-4752-a8ba-6ef2aab1576b"));

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "OrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("09de8588-5900-4caa-b6ea-a8e72517945b"), null, "User", "User" },
                    { new Guid("2645ed37-c27f-43c8-8b6a-917e96c22a53"), null, "Staff", "Staff" },
                    { new Guid("ee36f2b6-47a2-4f36-a512-d8b8522e1450"), null, "Shop", "Shop" },
                    { new Guid("fb435ef1-1b14-434c-9f05-a317fd5e7225"), null, "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("32a0b04a-6f78-4255-9253-a9a8646d8701"), new DateTime(2024, 9, 23, 16, 15, 3, 695, DateTimeKind.Local).AddTicks(3993), null, "Rice", null, false, new DateTime(2024, 9, 23, 16, 15, 3, 695, DateTimeKind.Local).AddTicks(4006), "Rice" },
                    { new Guid("d7e38ff9-346b-461d-b333-841765a8babc"), new DateTime(2024, 9, 23, 16, 15, 3, 695, DateTimeKind.Local).AddTicks(4010), null, "SuShi", null, false, new DateTime(2024, 9, 23, 16, 15, 3, 695, DateTimeKind.Local).AddTicks(4011), "SuShi" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("09de8588-5900-4caa-b6ea-a8e72517945b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2645ed37-c27f-43c8-8b6a-917e96c22a53"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee36f2b6-47a2-4f36-a512-d8b8522e1450"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("fb435ef1-1b14-434c-9f05-a317fd5e7225"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("32a0b04a-6f78-4255-9253-a9a8646d8701"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d7e38ff9-346b-461d-b333-841765a8babc"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("515e9800-fc1b-4969-bfc6-12e1312e4c70"), null, "Shop", "Shop" },
                    { new Guid("8caf1fe3-6485-4a51-a182-17905d06fa41"), null, "Staff", "Staff" },
                    { new Guid("b6da9c71-11fa-4385-a880-43883a779805"), null, "Admin", "Admin" },
                    { new Guid("d65c7293-8646-4cdb-9949-2ffa014b6b64"), null, "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("435d4e27-87c1-4adc-b0de-972ac53304e3"), new DateTime(2024, 9, 21, 20, 40, 26, 147, DateTimeKind.Local).AddTicks(425), null, "Rice", null, false, new DateTime(2024, 9, 21, 20, 40, 26, 147, DateTimeKind.Local).AddTicks(437), "Rice" },
                    { new Guid("ccb39ba3-340e-4752-a8ba-6ef2aab1576b"), new DateTime(2024, 9, 21, 20, 40, 26, 147, DateTimeKind.Local).AddTicks(464), null, "SuShi", null, false, new DateTime(2024, 9, 21, 20, 40, 26, 147, DateTimeKind.Local).AddTicks(465), "SuShi" }
                });
        }
    }
}
