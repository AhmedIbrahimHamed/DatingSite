using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingSite.API.Migrations
{
    public partial class FixedColumnIntroductionInUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Interoduction",
                table: "Users",
                newName: "Introduction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Introduction",
                table: "Users",
                newName: "Interoduction");
        }
    }
}
