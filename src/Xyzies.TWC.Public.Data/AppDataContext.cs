using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Entities.EntityConfigurations;
using Xyzies.TWC.Public.Data.Entities.ServiceProvider;
using Xyzies.TWC.Public.Data.Entities.Tenant;

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
        public DbSet<ProviderSetting> ProvidersSetting { get; set; }
        
        #endregion
    }
}
