using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sep3DataTier.Migrations
{
    public partial class InitializedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a313f1cd-e3c2-45c8-a1c7-548483b0dfc0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d816ca62-e3ee-4cb8-afb8-0da38165c478");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "19934293-146d-4ab2-8f53-1d4588613c10", "48711bc9-9ea0-486c-b0d5-f4dbcc24f543", "User", "USER" },
                    { "642df21f-11aa-4b86-a456-8576f0b82b95", "5b00611c-e858-4565-abb0-6721cf874a4e", "Admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "a313f1cd-e3c2-45c8-a1c7-548483b0dfc0", "32b3e80f-c96a-465e-a7ff-f79f4a011dee", "USER", null },
                    { "d816ca62-e3ee-4cb8-afb8-0da38165c478", "c9327834-07d5-4564-8972-a58e57d8b06b", "ADMIN", null }
                });
        }
    }
}
