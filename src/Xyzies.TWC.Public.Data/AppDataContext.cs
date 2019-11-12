using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Entities.EntityConfigurations;
using Xyzies.TWC.Public.Data.Entities.ServiceProvider;

namespace Xyzies.TWC.Public.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {

        }

        #region Entities
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<CompanyServiceProvider> CompanyServiceProviders { get; set; }
        public DbSet<ProviderSetting> ProvidersSetting { get; set; }

        #endregion
    }
}
