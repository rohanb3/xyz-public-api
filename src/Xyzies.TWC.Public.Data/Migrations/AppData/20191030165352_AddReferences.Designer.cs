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
    [Migration("20191030165352_AddReferences")]
    partial class AddReferences
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Xyzies.TWC.Public.Data.Entities.ServiceProvider.CompanyServiceProvider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<Guid>("ServiceProviderId");

                    b.HasKey("Id");

                    b.HasIndex("ServiceProviderId");

                    b.ToTable("CompanyServiceProviders");
                });

            modelBuilder.Entity("Xyzies.TWC.Public.Data.Entities.ServiceProvider.ServiceProvider", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("SeviceProviderName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ServiceProviders");
                });

            modelBuilder.Entity("Xyzies.TWC.Public.Data.Entities.ServiceProvider.CompanyServiceProvider", b =>
                {
                    b.HasOne("Xyzies.TWC.Public.Data.Entities.ServiceProvider.ServiceProvider", "ServiceProvider")
                        .WithMany("CompanyIds")
                        .HasForeignKey("ServiceProviderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
