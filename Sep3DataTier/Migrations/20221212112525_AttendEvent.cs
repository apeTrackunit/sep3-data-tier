using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sep3DataTier.Migrations
{
    public partial class AttendEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_OrganiserId",
                table: "Events");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a2d8628-1135-467b-a920-94aab56ce557");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3c6d25e-5440-4c60-be76-10c43cfa31ff");

            migrationBuilder.AlterColumn<string>(
                name: "OrganiserId",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationUserEvent",
                columns: table => new
                {
                    AttendedEventsId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttendeesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserEvent", x => new { x.AttendedEventsId, x.AttendeesId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserEvent_AspNetUsers_AttendeesId",
                        column: x => x.AttendeesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserEvent_Events_AttendedEventsId",
                        column: x => x.AttendedEventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserEvent_AttendeesId",
                table: "ApplicationUserEvent",
                column: "AttendeesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_OrganiserId",
                table: "Events",
                column: "OrganiserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_OrganiserId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "ApplicationUserEvent");

            migrationBuilder.AlterColumn<string>(
                name: "OrganiserId",
                table: "Events",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6a2d8628-1135-467b-a920-94aab56ce557", "91020c98-1222-484a-b291-5ed932cb85b6", "Admin", "ADMIN" },
                    { "f3c6d25e-5440-4c60-be76-10c43cfa31ff", "5c4a001a-5741-43c7-af51-895cea813582", "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EventId",
                table: "AspNetUsers",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Events_EventId",
                table: "AspNetUsers",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_OrganiserId",
                table: "Events",
                column: "OrganiserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
