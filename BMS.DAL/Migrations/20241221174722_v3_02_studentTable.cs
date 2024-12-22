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

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "StudentApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StatusStudent",
                table: "StudentApplications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isOpenToday",
                table: "OpeningHours",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                    { new Guid("73763b56-92a1-41f3-aaa9-fe6c8dad745c"), null, "Staff", "Staff" },
                    { new Guid("75c5e41d-38c8-40f6-8e97-f1c661b5631a"), null, "Shop", "Shop" },
                    { new Guid("9691b818-23ab-48bf-9deb-a965c55711c6"), null, "User", "User" },
                    { new Guid("a56093b9-12d2-483e-90f6-0d413e910d69"), null, "Admin", "Admin" }
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
                keyValue: new Guid("73763b56-92a1-41f3-aaa9-fe6c8dad745c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("75c5e41d-38c8-40f6-8e97-f1c661b5631a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9691b818-23ab-48bf-9deb-a965c55711c6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a56093b9-12d2-483e-90f6-0d413e910d69"));

            migrationBuilder.DropColumn(
                name: "Email",
                table: "StudentApplications");

            migrationBuilder.DropColumn(
                name: "StatusStudent",
                table: "StudentApplications");

            migrationBuilder.DropColumn(
                name: "isOpenToday",
                table: "OpeningHours");

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
