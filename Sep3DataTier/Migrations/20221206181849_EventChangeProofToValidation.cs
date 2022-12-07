using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sep3DataTier.Migrations
{
    public partial class EventChangeProofToValidation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DeleteData(
            //     table: "AspNetRoles",
            //     keyColumn: "Id",
            //     keyValue: "4540e966-9173-4899-94aa-cdc09727f861");
            //
            // migrationBuilder.DeleteData(
            //     table: "AspNetRoles",
            //     keyColumn: "Id",
            //     keyValue: "4bab06a0-adf6-4963-835e-609f4a21fc9f");

            migrationBuilder.RenameColumn(
                name: "Proof",
                table: "Events",
                newName: "Validation");

            // migrationBuilder.InsertData(
            //     table: "AspNetRoles",
            //     columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //     values: new object[,]
            //     {
            //         { "1a40ac17-217d-4f0c-909c-f66efdb0dc2c", "e7b34338-1ca8-4c1c-8272-55ea43e95fd8", "User", "USER" },
            //         { "7421f6b8-b8d3-4862-b2a3-22c0801310e8", "27aeb389-3353-4ebd-976b-288692e96371", "Admin", "ADMIN" }
            //     });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DeleteData(
            //     table: "AspNetRoles",
            //     keyColumn: "Id",
            //     keyValue: "1a40ac17-217d-4f0c-909c-f66efdb0dc2c");
            //
            // migrationBuilder.DeleteData(
            //     table: "AspNetRoles",
            //     keyColumn: "Id",
            //     keyValue: "7421f6b8-b8d3-4862-b2a3-22c0801310e8");

            migrationBuilder.RenameColumn(
                name: "Validation",
                table: "Events",
                newName: "Proof");

            // migrationBuilder.InsertData(
            //     table: "AspNetRoles",
            //     columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //     values: new object[,]
            //     {
            //         { "4540e966-9173-4899-94aa-cdc09727f861", "8562d7f3-e7b1-4175-ac22-16a930ab5c0e", "User", "USER" },
            //         { "4bab06a0-adf6-4963-835e-609f4a21fc9f", "e800b944-abf1-4393-b24d-102ecdffc82e", "Admin", "ADMIN" }
            //     });
        }
    }
}
