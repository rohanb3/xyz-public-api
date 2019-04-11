using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xyzies.TWC.Public.Data.Migrations
{
    public partial class BranchTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TWC_BranchContactType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TWC_BranchContactType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TWC_Companies",
                columns: table => new
                {
                    CompanyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyName = table.Column<string>(nullable: true),
                    LegalName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    StoreID = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    Agentid = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    StoreLocationCount = table.Column<int>(nullable: true),
                    PrimaryContactName = table.Column<string>(nullable: true),
                    PrimaryContactTitle = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    GeoLat = table.Column<string>(nullable: true),
                    GeoLon = table.Column<string>(nullable: true),
                    FedId = table.Column<string>(nullable: true),
                    TypeOfCompany = table.Column<int>(nullable: true),
                    StateEstablished = table.Column<string>(nullable: true),
                    CompanyType = table.Column<byte>(nullable: true),
                    CallerId = table.Column<string>(nullable: true),
                    IsAgreement = table.Column<bool>(nullable: true),
                    ActivityStatus = table.Column<string>(nullable: true),
                    CompanyKey = table.Column<Guid>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    CellNumber = table.Column<string>(nullable: true),
                    BankNumber = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    BankAccountNumber = table.Column<string>(nullable: true),
                    XyziesId = table.Column<string>(nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: true),
                    BankInfoGiven = table.Column<bool>(nullable: true),
                    AccountManager = table.Column<int>(nullable: true),
                    CrmCompanyId = table.Column<string>(nullable: true),
                    IsCallCenter = table.Column<bool>(nullable: true),
                    ParentCompanyId = table.Column<int>(nullable: true),
                    TeamKey = table.Column<Guid>(nullable: true),
                    RetailerGroupKey = table.Column<Guid>(nullable: true),
                    SocialMediaAccount = table.Column<string>(nullable: true),
                    RetailerGoogleAccount = table.Column<string>(nullable: true),
                    RetailerGooglePassword = table.Column<string>(nullable: true),
                    PaymentMode = table.Column<int>(nullable: true),
                    CustomerDemographicId = table.Column<int>(nullable: true),
                    LocationTypeId = table.Column<int>(nullable: true),
                    IsOwnerPassBackground = table.Column<bool>(nullable: true),
                    IsWebsite = table.Column<bool>(nullable: true),
                    IsSellsLifelineWireless = table.Column<bool>(nullable: true),
                    NumberofStores = table.Column<int>(nullable: true),
                    BusinessDescription = table.Column<string>(nullable: true),
                    WebsiteList = table.Column<string>(nullable: true),
                    IsSpectrum = table.Column<bool>(nullable: true),
                    BusinessSource = table.Column<int>(nullable: true),
                    IsMarketPlace = table.Column<bool>(nullable: true),
                    MarketPlaceName = table.Column<string>(nullable: true),
                    PhysicalName = table.Column<string>(nullable: true),
                    MarketStrategy = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CompanyID", x => x.CompanyID);
                });

            migrationBuilder.CreateTable(
                name: "TWC_Branch",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BranchName = table.Column<string>(maxLength: 256, nullable: false),
                    Email = table.Column<string>(maxLength: 128, nullable: true),
                    Phone = table.Column<string>(maxLength: 16, nullable: true),
                    Fax = table.Column<string>(maxLength: 16, nullable: true),
                    State = table.Column<string>(maxLength: 64, nullable: true),
                    City = table.Column<string>(maxLength: 64, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 16, nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    GeoLat = table.Column<string>(maxLength: 32, nullable: true),
                    GeoLng = table.Column<string>(maxLength: 32, nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TWC_Branch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TWC_Branch_TWC_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "TWC_Companies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TWC_BranchContact",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PersonName = table.Column<string>(maxLength: 50, nullable: true),
                    PersonLastName = table.Column<string>(maxLength: 50, nullable: true),
                    PersonTitle = table.Column<string>(maxLength: 100, nullable: true),
                    Value = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    BranchContactTypeId = table.Column<Guid>(nullable: false),
                    BranchId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TWC_BranchContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TWC_BranchContact_TWC_BranchContactType_BranchContactTypeId",
                        column: x => x.BranchContactTypeId,
                        principalTable: "TWC_BranchContactType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TWC_BranchContact_TWC_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "TWC_Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TWC_Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    SalesPersonID = table.Column<int>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Imagename = table.Column<string>(nullable: true),
                    UserRefID = table.Column<int>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Is_Agreement = table.Column<bool>(nullable: true),
                    XyziesId = table.Column<string>(nullable: true),
                    ManagedBy = table.Column<int>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    UserGuid = table.Column<Guid>(nullable: true),
                    IPAddressRestriction = table.Column<string>(nullable: true),
                    SocialMediaAccount = table.Column<string>(nullable: true),
                    PhotoID = table.Column<string>(nullable: true),
                    PasswordExpiryOn = table.Column<DateTime>(nullable: true),
                    LoginIpAddress = table.Column<string>(nullable: true),
                    IsPhoneVerified = table.Column<bool>(nullable: true),
                    IsIdentityUploaded = table.Column<bool>(nullable: true),
                    IsEmailVerified = table.Column<bool>(nullable: true),
                    IsUserPictureUploaded = table.Column<bool>(nullable: true),
                    StatusId = table.Column<int>(nullable: true),
                    BranchID = table.Column<Guid>(nullable: true),
                    CompanyID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UserID", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_TWC_Users_TWC_Branch_BranchID",
                        column: x => x.BranchID,
                        principalTable: "TWC_Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TWC_Users_TWC_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "TWC_Companies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TWC_Branch_CompanyId",
                table: "TWC_Branch",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TWC_BranchContact_BranchContactTypeId",
                table: "TWC_BranchContact",
                column: "BranchContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TWC_BranchContact_BranchId",
                table: "TWC_BranchContact",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TWC_Users_BranchID",
                table: "TWC_Users",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_TWC_Users_CompanyID",
                table: "TWC_Users",
                column: "CompanyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TWC_BranchContact");

            migrationBuilder.DropTable(
                name: "TWC_Users");

            migrationBuilder.DropTable(
                name: "TWC_BranchContactType");

            migrationBuilder.DropTable(
                name: "TWC_Branch");

            migrationBuilder.DropTable(
                name: "TWC_Companies");
        }
    }
}
