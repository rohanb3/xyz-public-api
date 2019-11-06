using Microsoft.EntityFrameworkCore.Migrations;

namespace Xyzies.TWC.Public.Data.Migrations.AppData
{
    public partial class Typo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SeviceProviderName",
                table: "ServiceProviders",
                newName: "ServiceProviderName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceProviderName",
                table: "ServiceProviders",
                newName: "SeviceProviderName");
        }
    }
}
