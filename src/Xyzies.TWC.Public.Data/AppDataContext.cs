using Microsoft.EntityFrameworkCore;
using System;
using Xyzies.TWC.Public.Data.Entities.TenantEntities;
using Xyzies.TWC.Public.Data.Entities.TenantEntities.EntityConfigurations;

namespace Xyzies.TWC.Public.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {

        }

        #region Entities
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<CompanyTenant> CompanyTenants { get; set; }
        public DbSet<TenantSetting> TenantsSetting { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TenantConfiguration());
        }
    }
}
