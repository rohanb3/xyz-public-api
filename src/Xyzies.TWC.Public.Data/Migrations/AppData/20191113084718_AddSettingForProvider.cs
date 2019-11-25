using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xyzies.TWC.Public.Data.Migrations.AppData
{
    public partial class AddSettingForProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenantsSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Settings = table.Column<string>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantsSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantsSetting_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantsSetting_TenantId",
                table: "TenantsSetting",
                column: "TenantId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProvidersSetting");
        }
    }
}
