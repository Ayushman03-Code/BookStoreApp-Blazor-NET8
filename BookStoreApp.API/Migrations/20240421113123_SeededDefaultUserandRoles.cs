using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUserandRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "85b56d87-6af5-4900-b3d1-e902da6f0316", null, "User", "USER" },
                    { "b7617023-bd2e-45f6-bb09-cee969f6f456", null, "Admin", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "c2ff4ddd-1c99-4fd6-9af0-7d5b6dd591fc", 0, "89e65988-1c0c-4088-8263-8c705dd0001b", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEGcNMviKYk0tV8tYLp9a3HURKL21B8V1T9OQDWfTPZwyseiyXo6MY3Nqvn9XMG9B+g==", null, false, "4289991a-7a82-4acd-ba86-d12cd661a328", false, "admin@bookstore.com" },
                    { "e94a3f42-0068-4564-ae6e-ecc4e344a988", 0, "9cb8ecd6-a85c-492a-8490-b15c8da6ed57", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEN85y4ZIDBfSQLTiHLUqW7jvTpRxOniQ+7r/r+kFnxD+T1RyCjDSBBkT1QZjNGgc8w==", null, false, "beab38d5-b804-49c8-af84-4b96bc736ed7", false, "user@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "b7617023-bd2e-45f6-bb09-cee969f6f456", "c2ff4ddd-1c99-4fd6-9af0-7d5b6dd591fc" },
                    { "85b56d87-6af5-4900-b3d1-e902da6f0316", "e94a3f42-0068-4564-ae6e-ecc4e344a988" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b7617023-bd2e-45f6-bb09-cee969f6f456", "c2ff4ddd-1c99-4fd6-9af0-7d5b6dd591fc" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "85b56d87-6af5-4900-b3d1-e902da6f0316", "e94a3f42-0068-4564-ae6e-ecc4e344a988" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85b56d87-6af5-4900-b3d1-e902da6f0316");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7617023-bd2e-45f6-bb09-cee969f6f456");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c2ff4ddd-1c99-4fd6-9af0-7d5b6dd591fc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e94a3f42-0068-4564-ae6e-ecc4e344a988");
        }
    }
}
