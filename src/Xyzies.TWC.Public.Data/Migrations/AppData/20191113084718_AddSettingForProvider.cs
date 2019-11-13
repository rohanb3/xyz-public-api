using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xyzies.TWC.Public.Data.Migrations.AppData
{
    public partial class AddSettingForProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProvidersSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Settings = table.Column<string>(nullable: true),
                    ServiceProviderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvidersSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProvidersSetting_ServiceProviders_ServiceProviderId",
                        column: x => x.ServiceProviderId,
                        principalTable: "ServiceProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProvidersSetting_ServiceProviderId",
                table: "ProvidersSetting",
                column: "ServiceProviderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProvidersSetting");
        }
    }
}
