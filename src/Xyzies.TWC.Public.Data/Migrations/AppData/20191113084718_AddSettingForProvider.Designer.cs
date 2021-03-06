﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Xyzies.TWC.Public.Data;

namespace Xyzies.TWC.Public.Data.Migrations.AppData
{
    [DbContext(typeof(AppDataContext))]
    [Migration("20191113084718_AddSettingForProvider")]
    partial class AddSettingForProvider
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Xyzies.TWC.Public.Data.Entities.TenantEntities.CompanyTenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<Guid>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.ToTable("CompanyTenants");
                });

            modelBuilder.Entity("Xyzies.TWC.Public.Data.Entities.TenantEntities.TenantSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("TenantId");

                    b.Property<string>("Settings");

                    b.HasKey("Id");

                    b.HasIndex("TenantId")
                        .IsUnique();

                    b.ToTable("ProvidersSetting");
                });

            modelBuilder.Entity("Xyzies.TWC.Public.Data.Entities.TenantEntities.Tenant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("TenantName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("Xyzies.TWC.Public.Data.Entities.TenantEntities.CompanyTenant", b =>
                {
                    b.HasOne("Xyzies.TWC.Public.Data.Entities.TenantEntities.Tenant", "Tenant")
                        .WithMany("Companies")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Xyzies.TWC.Public.Data.Entities.TenantEntities.TenantSetting", b =>
                {
                    b.HasOne("Xyzies.TWC.Public.Data.Entities.TenantEntities.Tenant", "Tenant")
                        .WithOne("TenantSetting")
                        .HasForeignKey("Xyzies.TWC.Public.Data.Entities.TenantEntities.TenantSetting", "TenantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
