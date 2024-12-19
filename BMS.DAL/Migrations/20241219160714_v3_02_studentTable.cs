using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                name: "IX_StudentApplications_UserId",
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
                name: "ImageId",
                table: "StudentApplications",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_StudentApplications_ImageId",
                table: "StudentApplications",
                newName: "IX_StudentApplications_UserId1");

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

            migrationBuilder.AddForeignKey(
                name: "FK_StudentApplications_AspNetUsers_UserId1",
                table: "StudentApplications",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentApplications_AspNetUsers_UserId1",
                table: "StudentApplications");

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
                name: "UserId1",
                table: "StudentApplications",
                newName: "ImageId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "StudentApplications",
                newName: "UniversityName");

            migrationBuilder.RenameColumn(
                name: "ImageCardStudent",
                table: "StudentApplications",
                newName: "MSSV");

            migrationBuilder.RenameIndex(
                name: "IX_StudentApplications_UserId1",
                table: "StudentApplications",
                newName: "IX_StudentApplications_ImageId");

            migrationBuilder.RenameColumn(
                name: "StudentIdCard",
                table: "AspNetUsers",
                newName: "IdStudent");

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
