using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sep3DataTier.Migrations
{
    public partial class EventStatusRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateOnly = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeOnly = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Validation = table.Column<byte[]>(type: "bytea", nullable: true),
                    OrganiserId = table.Column<string>(type: "text", nullable: true),
                    ReportId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_OrganiserId",
                        column: x => x.OrganiserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Events_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // migrationBuilder.InsertData(
            //     table: "AspNetRoles",
            //     columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //     values: new object[,]
            //     {
            //         { "276da8e3-2222-4fd5-bcff-2deb2c62e610", "f65a240d-3aca-4673-8f04-df7648139280", "User", "USER" },
            //         { "54fedc6c-612e-4f84-b4e9-e8de24c6519e", "9c14d40c-1495-4529-a62f-473298b7312b", "Admin", "ADMIN" }
            //     });

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganiserId",
                table: "Events",
                column: "OrganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ReportId",
                table: "Events",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            // migrationBuilder.DeleteData(
            //     table: "AspNetRoles",
            //     keyColumn: "Id",
            //     keyValue: "276da8e3-2222-4fd5-bcff-2deb2c62e610");
            //
            // migrationBuilder.DeleteData(
            //     table: "AspNetRoles",
            //     keyColumn: "Id",
            //     keyValue: "54fedc6c-612e-4f84-b4e9-e8de24c6519e");
        }
    }
}
