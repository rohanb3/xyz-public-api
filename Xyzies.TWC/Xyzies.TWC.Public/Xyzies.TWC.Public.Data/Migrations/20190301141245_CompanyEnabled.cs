using Microsoft.EntityFrameworkCore.Migrations;

namespace Xyzies.TWC.Public.Data.Migrations
{
    public partial class CompanyEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDisabled",
                table: "TWC_Branches",
                newName: "IsEnabled");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "TWC_Companies",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "TWC_Companies");

            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "TWC_Branches",
                newName: "IsDisabled");
        }
    }
}
