using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sep3DataTier.Migrations
{
    public partial class AddApproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Events"); 
        }
    }
}
