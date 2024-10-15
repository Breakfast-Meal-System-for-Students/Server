using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v108_ChangeTypeOfQrCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_CartGroupUser_CartGroupUserId",
                table: "CartDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CartGroupUser_AspNetUsers_UserId",
                table: "CartGroupUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CartGroupUser_Carts_CartId",
                table: "CartGroupUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartGroupUser",
                table: "CartGroupUser");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0013724d-f87e-4fca-a2fd-631f017b5ca4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0d6398de-b26b-4ff7-9da4-4229f66ab0bc"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1d02915f-75c4-452d-ad96-1c2d5994d3ad"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("74adf4f9-4b30-4d8a-bb10-e246ae652c7a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("32afa54c-8915-4bc1-b7e5-ab7183249895"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("394aa975-db26-4742-81c1-9a3536e4e87c"));

            migrationBuilder.RenameTable(
                name: "CartGroupUser",
                newName: "CartGroupUsers");

            migrationBuilder.RenameIndex(
                name: "IX_CartGroupUser_UserId",
                table: "CartGroupUsers",
                newName: "IX_CartGroupUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CartGroupUser_CartId",
                table: "CartGroupUsers",
                newName: "IX_CartGroupUsers_CartId");

            migrationBuilder.AlterColumn<string>(
                name: "QRCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartGroupUsers",
                table: "CartGroupUsers",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("20f0dacb-4a29-4819-82ab-1e011bd7b3e7"), null, "User", "User" },
                    { new Guid("25e96c02-f1f9-424e-835b-e2b69de69188"), null, "Admin", "Admin" },
                    { new Guid("a02e0b2b-a82b-4b80-940e-26d37b175cef"), null, "Shop", "Shop" },
                    { new Guid("b1bf0e23-05d5-43bb-9812-cc78568b2538"), null, "Staff", "Staff" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("3b6e6e51-2075-48df-b7d8-337df3fe6891"), new DateTime(2024, 10, 15, 10, 48, 19, 508, DateTimeKind.Local).AddTicks(9049), null, "SuShi", null, false, new DateTime(2024, 10, 15, 10, 48, 19, 508, DateTimeKind.Local).AddTicks(9050), "SuShi" },
                    { new Guid("61a42337-0955-4d40-aa6a-533e6ab3ae34"), new DateTime(2024, 10, 15, 10, 48, 19, 508, DateTimeKind.Local).AddTicks(9031), null, "Rice", null, false, new DateTime(2024, 10, 15, 10, 48, 19, 508, DateTimeKind.Local).AddTicks(9042), "Rice" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_CartGroupUsers_CartGroupUserId",
                table: "CartDetails",
                column: "CartGroupUserId",
                principalTable: "CartGroupUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartGroupUsers_AspNetUsers_UserId",
                table: "CartGroupUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartGroupUsers_Carts_CartId",
                table: "CartGroupUsers",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_CartGroupUsers_CartGroupUserId",
                table: "CartDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CartGroupUsers_AspNetUsers_UserId",
                table: "CartGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CartGroupUsers_Carts_CartId",
                table: "CartGroupUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartGroupUsers",
                table: "CartGroupUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("20f0dacb-4a29-4819-82ab-1e011bd7b3e7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("25e96c02-f1f9-424e-835b-e2b69de69188"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a02e0b2b-a82b-4b80-940e-26d37b175cef"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b1bf0e23-05d5-43bb-9812-cc78568b2538"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3b6e6e51-2075-48df-b7d8-337df3fe6891"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("61a42337-0955-4d40-aa6a-533e6ab3ae34"));

            migrationBuilder.RenameTable(
                name: "CartGroupUsers",
                newName: "CartGroupUser");

            migrationBuilder.RenameIndex(
                name: "IX_CartGroupUsers_UserId",
                table: "CartGroupUser",
                newName: "IX_CartGroupUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CartGroupUsers_CartId",
                table: "CartGroupUser",
                newName: "IX_CartGroupUser_CartId");

            migrationBuilder.AlterColumn<byte[]>(
                name: "QRCode",
                table: "Orders",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartGroupUser",
                table: "CartGroupUser",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0013724d-f87e-4fca-a2fd-631f017b5ca4"), null, "Shop", "Shop" },
                    { new Guid("0d6398de-b26b-4ff7-9da4-4229f66ab0bc"), null, "Admin", "Admin" },
                    { new Guid("1d02915f-75c4-452d-ad96-1c2d5994d3ad"), null, "User", "User" },
                    { new Guid("74adf4f9-4b30-4d8a-bb10-e246ae652c7a"), null, "Staff", "Staff" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "DeletedDate", "Description", "Image", "IsDeleted", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("32afa54c-8915-4bc1-b7e5-ab7183249895"), new DateTime(2024, 10, 11, 10, 13, 45, 577, DateTimeKind.Local).AddTicks(4935), null, "Rice", null, false, new DateTime(2024, 10, 11, 10, 13, 45, 577, DateTimeKind.Local).AddTicks(4947), "Rice" },
                    { new Guid("394aa975-db26-4742-81c1-9a3536e4e87c"), new DateTime(2024, 10, 11, 10, 13, 45, 577, DateTimeKind.Local).AddTicks(4951), null, "SuShi", null, false, new DateTime(2024, 10, 11, 10, 13, 45, 577, DateTimeKind.Local).AddTicks(4951), "SuShi" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_CartGroupUser_CartGroupUserId",
                table: "CartDetails",
                column: "CartGroupUserId",
                principalTable: "CartGroupUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartGroupUser_AspNetUsers_UserId",
                table: "CartGroupUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartGroupUser_Carts_CartId",
                table: "CartGroupUser",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
