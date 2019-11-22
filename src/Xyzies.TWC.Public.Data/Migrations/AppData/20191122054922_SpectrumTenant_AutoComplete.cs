using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xyzies.TWC.Public.Data.Migrations.AppData
{
    public partial class SpectrumTenant_AutoComplete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "CreatedOn", "Phone", "TenantName" },
                values: new object[] { new Guid("0ed21401-e0e6-4b22-aa89-4c5522212b67"), new DateTime(2019, 11, 22, 5, 49, 22, 39, DateTimeKind.Utc).AddTicks(6171), "380938821599", "Spectrum" });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("0ed21401-e0e6-4b22-aa89-4c5522212b67"));
        }
    }
}
