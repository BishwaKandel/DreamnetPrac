using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newtest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
