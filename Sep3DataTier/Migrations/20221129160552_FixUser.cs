using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sep3DataTier.Migrations
{
    public partial class FixUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19934293-146d-4ab2-8f53-1d4588613c10");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "642df21f-11aa-4b86-a456-8576f0b82b95");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "12f6168d-18fc-4dfb-ad18-9d9c3d1396d8", "6cf2afc0-8675-41b6-84a7-6a9a61c22a5a", "User", "USER" },
                    { "c2e9abd6-2363-4b2e-a235-b63765ed6c6c", "64990155-7b2f-457b-9a89-d8dfc862873a", "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12f6168d-18fc-4dfb-ad18-9d9c3d1396d8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2e9abd6-2363-4b2e-a235-b63765ed6c6c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "19934293-146d-4ab2-8f53-1d4588613c10", "48711bc9-9ea0-486c-b0d5-f4dbcc24f543", "User", "USER" },
                    { "642df21f-11aa-4b86-a456-8576f0b82b95", "5b00611c-e858-4565-abb0-6721cf874a4e", "Admin", "ADMIN" }
                });
        }
    }
}
