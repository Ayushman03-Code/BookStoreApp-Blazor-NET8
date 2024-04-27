using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUserandRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7617023-bd2e-45f6-bb09-cee969f6f456",
                column: "Name",
                value: "Administrator");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c2ff4ddd-1c99-4fd6-9af0-7d5b6dd591fc",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "22717caf-f994-40a1-b07d-98efb2fcb319", "AQAAAAIAAYagAAAAECnw0MxlPJAEDxcKF+IrLr5Bap79wD3onwiHgiwxD00UlE8bV09afGsyDCfGLjbEQg==", "cea6a233-b7ac-4276-8c31-57384c85c617" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e94a3f42-0068-4564-ae6e-ecc4e344a988",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "46c59062-3228-4efe-8cfe-6f9513341229", "AQAAAAIAAYagAAAAEIuLwSwsQhqWJyM5S0QvMwI+jmPI7zAd1wrawVyDWAEKvIEHZTpdu75/4G7tgiZ76w==", "a1ed4f52-4751-4eba-933d-459dc888b634" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7617023-bd2e-45f6-bb09-cee969f6f456",
                column: "Name",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c2ff4ddd-1c99-4fd6-9af0-7d5b6dd591fc",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "89e65988-1c0c-4088-8263-8c705dd0001b", "AQAAAAIAAYagAAAAEGcNMviKYk0tV8tYLp9a3HURKL21B8V1T9OQDWfTPZwyseiyXo6MY3Nqvn9XMG9B+g==", "4289991a-7a82-4acd-ba86-d12cd661a328" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e94a3f42-0068-4564-ae6e-ecc4e344a988",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9cb8ecd6-a85c-492a-8490-b15c8da6ed57", "AQAAAAIAAYagAAAAEN85y4ZIDBfSQLTiHLUqW7jvTpRxOniQ+7r/r+kFnxD+T1RyCjDSBBkT1QZjNGgc8w==", "beab38d5-b804-49c8-af84-4b96bc736ed7" });
        }
    }
}
