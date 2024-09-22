using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v102_DROPFKSHOP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisterCategorys_Products_ShopId",
                table: "RegisterCategorys");

            migrationBuilder.DropForeignKey(
                name: "FK_RegisterCategorys_Shops_ShopId",
                table: "RegisterCategorys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RegisterCategorys",
                table: "RegisterCategorys");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("766f193a-b84c-42ab-9a84-cf3f01badeb2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("872a2662-7b25-404d-af52-a8868feae13d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cba0632d-ac59-4374-a044-0f33d7a3dfe1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("fd9d7262-f7bd-4c0f-b6c2-222c889661d6"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("776eb3ad-5f64-4cba-afec-38ebcd37399e"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("8a0bb8ba-d798-4938-b0f8-156c21dbfab0"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ShopId",
                table: "RegisterCategorys",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RegisterCategorys",
                table: "RegisterCategorys",
                columns: new[] { "CategoryId", "ProductId" });

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

            migrationBuilder.CreateIndex(
                name: "IX_RegisterCategorys_ProductId",
                table: "RegisterCategorys",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisterCategorys_Products_ProductId",
                table: "RegisterCategorys",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegisterCategorys_Shops_ShopId",
                table: "RegisterCategorys",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisterCategorys_Products_ProductId",
                table: "RegisterCategorys");

            migrationBuilder.DropForeignKey(
                name: "FK_RegisterCategorys_Shops_ShopId",
                table: "RegisterCategorys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RegisterCategorys",
                table: "RegisterCategorys");

            migrationBuilder.DropIndex(
                name: "IX_RegisterCategorys_ProductId",
                table: "RegisterCategorys");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "ShopId",
                table: "RegisterCategorys",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RegisterCategorys",
                table: "RegisterCategorys",
                columns: new[] { "CategoryId", "ShopId", "ProductId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("766f193a-b84c-42ab-9a84-cf3f01badeb2"), null, "Staff", "Staff" },
                    { new Guid("872a2662-7b25-404d-af52-a8868feae13d"), null, "Shop", "Shop" },
                    { new Guid("cba0632d-ac59-4374-a044-0f33d7a3dfe1"), null, "Admin", "Admin" },
                    { new Guid("fd9d7262-f7bd-4c0f-b6c2-222c889661d6"), null, "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("776eb3ad-5f64-4cba-afec-38ebcd37399e"), new DateTime(2024, 9, 21, 10, 42, 40, 287, DateTimeKind.Local).AddTicks(3461), null, "Rice", null, false, new DateTime(2024, 9, 21, 10, 42, 40, 287, DateTimeKind.Local).AddTicks(3474), "Rice" },
                    { new Guid("8a0bb8ba-d798-4938-b0f8-156c21dbfab0"), new DateTime(2024, 9, 21, 10, 42, 40, 287, DateTimeKind.Local).AddTicks(3480), null, "SuShi", null, false, new DateTime(2024, 9, 21, 10, 42, 40, 287, DateTimeKind.Local).AddTicks(3480), "SuShi" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RegisterCategorys_Products_ShopId",
                table: "RegisterCategorys",
                column: "ShopId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegisterCategorys_Shops_ShopId",
                table: "RegisterCategorys",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
