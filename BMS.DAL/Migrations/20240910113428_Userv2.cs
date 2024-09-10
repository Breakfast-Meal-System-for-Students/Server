using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Userv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0444b601-248f-4c65-8806-665cd52a81f2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0af91399-1d7f-48b0-9e13-10b09095743f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("46484aa7-7667-4c12-beea-d2dadf66e948"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("70ebb2ac-4e92-46dd-8ab1-1bc8598c207b"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("225df778-d8e6-4d3b-bf23-25024dc64c51"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fcfaa15a-6412-486a-96fb-f5ae290994fa"));

            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                table: "Shops",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("43875937-c6ba-4646-85a0-0fda9bdcbac2"), null, "Shop", "Shop" },
                    { new Guid("841fcb90-7413-4c3f-ace0-3cf92e137de4"), null, "Admin", "Admin" },
                    { new Guid("84274534-15d4-466e-91be-43760f4634da"), null, "Staff", "Staff" },
                    { new Guid("a23dc72c-b152-40f3-b594-cda61e22ee2f"), null, "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "Description", "Image", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("58a1c65c-2e30-40e9-957e-75903f578e05"), new DateTime(2024, 9, 10, 18, 34, 28, 121, DateTimeKind.Local).AddTicks(5439), "SuShi", null, new DateTime(2024, 9, 10, 18, 34, 28, 121, DateTimeKind.Local).AddTicks(5439), "SuShi" },
                    { new Guid("e29ac1b3-58ed-4fa7-a31e-3a06609f1900"), new DateTime(2024, 9, 10, 18, 34, 28, 121, DateTimeKind.Local).AddTicks(5422), "Rice", null, new DateTime(2024, 9, 10, 18, 34, 28, 121, DateTimeKind.Local).AddTicks(5435), "Rice" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("43875937-c6ba-4646-85a0-0fda9bdcbac2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("841fcb90-7413-4c3f-ace0-3cf92e137de4"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("84274534-15d4-466e-91be-43760f4634da"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a23dc72c-b152-40f3-b594-cda61e22ee2f"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("58a1c65c-2e30-40e9-957e-75903f578e05"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e29ac1b3-58ed-4fa7-a31e-3a06609f1900"));

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Shops");

            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                table: "Shops",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0444b601-248f-4c65-8806-665cd52a81f2"), null, "User", "User" },
                    { new Guid("0af91399-1d7f-48b0-9e13-10b09095743f"), null, "Admin", "Admin" },
                    { new Guid("46484aa7-7667-4c12-beea-d2dadf66e948"), null, "Staff", "Staff" },
                    { new Guid("70ebb2ac-4e92-46dd-8ab1-1bc8598c207b"), null, "Shop", "Shop" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreateDate", "Description", "Image", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { new Guid("225df778-d8e6-4d3b-bf23-25024dc64c51"), new DateTime(2024, 9, 10, 12, 50, 53, 995, DateTimeKind.Local).AddTicks(3621), "SuShi", null, new DateTime(2024, 9, 10, 12, 50, 53, 995, DateTimeKind.Local).AddTicks(3622), "SuShi" },
                    { new Guid("fcfaa15a-6412-486a-96fb-f5ae290994fa"), new DateTime(2024, 9, 10, 12, 50, 53, 995, DateTimeKind.Local).AddTicks(3594), "Rice", null, new DateTime(2024, 9, 10, 12, 50, 53, 995, DateTimeKind.Local).AddTicks(3614), "Rice" }
                });
        }
    }
}
