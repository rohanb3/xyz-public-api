﻿using Microsoft.EntityFrameworkCore;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Entities.EntityConfigurations;

namespace Xyzies.TWC.Public.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {

        }

        #region Entities

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<BranchContact> PrimaryContacts { get; set; }

        public DbSet<BranchContactType> BranchContactTypes { get; set; }

        public DbSet<User> Users { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new BranchContactTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BranchConfiguration());
            modelBuilder.ApplyConfiguration(new BranchContactConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
