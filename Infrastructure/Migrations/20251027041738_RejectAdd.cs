using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RejectAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63bb6d6d-f91d-4e15-a821-a04f489799f2");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "55cac435-fb87-4a25-8ad1-43338e009407", "310c60fd-8a06-485a-8c53-88ab76bda28a" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "55cac435-fb87-4a25-8ad1-43338e009407");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "310c60fd-8a06-485a-8c53-88ab76bda28a");

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "454d3305-c2b5-45dc-ad8d-2a3dfc4890c3", null, "Admin", "ADMIN" },
                    { "87ae1a58-5e35-43ae-9257-dea9f4aa9b6b", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DOB", "DepartmentId", "Discriminator", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsDeleted", "JoiningDate", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Position", "ProfilePictureFileName", "Salary", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e81137e0-8603-48b4-aea7-2d58a62cd708", 0, "Head Office", "6888a067-f058-4049-98a9-a6d2d8f6f5fa", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "User", "admin@gmail.com", true, "System", true, false, new DateTime(2025, 10, 27, 4, 17, 37, 327, DateTimeKind.Utc).AddTicks(2395), "Admin", false, null, "System Admin", "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEDZVkLQ3CUrFHURmF8Mkjo5VO7kBDTyLcv1OtoKFibo15zEr8mOAM7HLZvOynuaSuw==", null, false, "Administrator", null, 0m, "67eeaac4-7732-435a-b72e-20b3de2380bf", false, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "454d3305-c2b5-45dc-ad8d-2a3dfc4890c3", "e81137e0-8603-48b4-aea7-2d58a62cd708" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87ae1a58-5e35-43ae-9257-dea9f4aa9b6b");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "454d3305-c2b5-45dc-ad8d-2a3dfc4890c3", "e81137e0-8603-48b4-aea7-2d58a62cd708" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "454d3305-c2b5-45dc-ad8d-2a3dfc4890c3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e81137e0-8603-48b4-aea7-2d58a62cd708");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "LeaveRequests");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "55cac435-fb87-4a25-8ad1-43338e009407", null, "Admin", "ADMIN" },
                    { "63bb6d6d-f91d-4e15-a821-a04f489799f2", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DOB", "DepartmentId", "Discriminator", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsDeleted", "JoiningDate", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Position", "ProfilePictureFileName", "Salary", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "310c60fd-8a06-485a-8c53-88ab76bda28a", 0, "Head Office", "40721c97-9d41-4a61-9b2d-1ff983e17409", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "User", "admin@gmail.com", true, "System", true, false, new DateTime(2025, 10, 26, 16, 43, 13, 649, DateTimeKind.Utc).AddTicks(2719), "Admin", false, null, "System Admin", "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEPqV57bl+JzxkVV22n4XLmnDyFpyQt9eVVRlUwIwCFTvAgPojYm5rDbIVM0VDBuiug==", null, false, "Administrator", null, 0m, "537ba0bc-541c-49a4-a893-08af74ef7b97", false, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "55cac435-fb87-4a25-8ad1-43338e009407", "310c60fd-8a06-485a-8c53-88ab76bda28a" });
        }
    }
}
