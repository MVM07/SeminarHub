using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeminarHub.Data.Migrations
{
    public partial class OrganiserColumnFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seminars_AspNetUsers_OrganizerId",
                table: "Seminars");

            migrationBuilder.DropIndex(
                name: "IX_Seminars_OrganizerId",
                table: "Seminars");

            migrationBuilder.DropColumn(
                name: "OrganizerId",
                table: "Seminars");

            migrationBuilder.AlterColumn<string>(
                name: "OrganiserId",
                table: "Seminars",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Seminars_OrganiserId",
                table: "Seminars",
                column: "OrganiserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seminars_AspNetUsers_OrganiserId",
                table: "Seminars",
                column: "OrganiserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seminars_AspNetUsers_OrganiserId",
                table: "Seminars");

            migrationBuilder.DropIndex(
                name: "IX_Seminars_OrganiserId",
                table: "Seminars");

            migrationBuilder.AlterColumn<string>(
                name: "OrganiserId",
                table: "Seminars",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "OrganizerId",
                table: "Seminars",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Seminars_OrganizerId",
                table: "Seminars",
                column: "OrganizerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seminars_AspNetUsers_OrganizerId",
                table: "Seminars",
                column: "OrganizerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
