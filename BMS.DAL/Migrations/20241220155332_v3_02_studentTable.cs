using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class v3_02_studentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentApplications_AspNetUsers_UserId",
                table: "StudentApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentApplications_Images_ImageId",
                table: "StudentApplications");

            migrationBuilder.DropIndex(
                name: "IX_StudentApplications_ImageId",
                table: "StudentApplications");

            migrationBuilder.DropIndex(
                name: "IX_StudentApplications_UserId",
                table: "StudentApplications");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "StudentApplications");

            migrationBuilder.RenameColumn(
                name: "UniversityName",
                table: "StudentApplications",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "MSSV",
                table: "StudentApplications",
                newName: "ImageCardStudent");

            migrationBuilder.RenameColumn(
                name: "IdStudent",
                table: "AspNetUsers",
                newName: "StudentIdCard");

            migrationBuilder.AddColumn<int>(
                name: "StatusStudent",
                table: "StudentApplications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentApplicationId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UniversityId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0408a1b2-c55a-4620-a414-ffa7849727ef"), null, "Shop", "Shop" },
                    { new Guid("beae467e-3d84-4c35-9469-e18d1b0959d7"), null, "User", "User" },
                    { new Guid("d1e2bed4-9a21-49ed-8b7a-11034e8527d7"), null, "Admin", "Admin" },
                    { new Guid("e6881f62-34af-464e-af7c-6936897b3a49"), null, "Staff", "Staff" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentApplications_AspNetUsers_Id",
                table: "StudentApplications",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentApplications_AspNetUsers_Id",
                table: "StudentApplications");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0408a1b2-c55a-4620-a414-ffa7849727ef"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("beae467e-3d84-4c35-9469-e18d1b0959d7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d1e2bed4-9a21-49ed-8b7a-11034e8527d7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e6881f62-34af-464e-af7c-6936897b3a49"));

            migrationBuilder.DropColumn(
                name: "StatusStudent",
                table: "StudentApplications");

            migrationBuilder.DropColumn(
                name: "StudentApplicationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "StudentApplications",
                newName: "UniversityName");

            migrationBuilder.RenameColumn(
                name: "ImageCardStudent",
                table: "StudentApplications",
                newName: "MSSV");

            migrationBuilder.RenameColumn(
                name: "StudentIdCard",
                table: "AspNetUsers",
                newName: "IdStudent");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "StudentApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_StudentApplications_ImageId",
                table: "StudentApplications",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentApplications_UserId",
                table: "StudentApplications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentApplications_AspNetUsers_UserId",
                table: "StudentApplications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentApplications_Images_ImageId",
                table: "StudentApplications",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
