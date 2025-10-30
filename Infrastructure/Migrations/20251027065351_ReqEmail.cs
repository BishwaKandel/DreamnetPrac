using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReqEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "RequestedByEmail",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "067a3861-a2cc-4308-82be-d300f24a720f", null, "Admin", "ADMIN" },
                    { "48e44bd0-9ae4-4ee8-925f-a49d55cc2996", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DOB", "DepartmentId", "Discriminator", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsDeleted", "JoiningDate", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Position", "ProfilePictureFileName", "Salary", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ed9175cf-2c6e-42f1-a60c-fe243fc3d1de", 0, "Head Office", "294e4751-bba0-49c7-9e87-606c9af040f0", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "User", "admin@gmail.com", true, "System", true, false, new DateTime(2025, 10, 27, 6, 53, 50, 881, DateTimeKind.Utc).AddTicks(774), "Admin", false, null, "System Admin", "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEAGo3hAyVF765NbqJbYJaYwQJ+hA6DpXlxzcchKett4wIKv+De7ZDKfryhTSzS9paQ==", null, false, "Administrator", null, 0m, "c1c4160c-5d1b-4f24-8360-67637cef1fe2", false, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "067a3861-a2cc-4308-82be-d300f24a720f", "ed9175cf-2c6e-42f1-a60c-fe243fc3d1de" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48e44bd0-9ae4-4ee8-925f-a49d55cc2996");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "067a3861-a2cc-4308-82be-d300f24a720f", "ed9175cf-2c6e-42f1-a60c-fe243fc3d1de" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "067a3861-a2cc-4308-82be-d300f24a720f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ed9175cf-2c6e-42f1-a60c-fe243fc3d1de");

            migrationBuilder.DropColumn(
                name: "RequestedByEmail",
                table: "LeaveRequests");

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
    }
}
