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

        #endregion

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //modelBuilder.ApplyConfiguration(new CompanyServiceProvidersConfigurations());
        //}
    }

    //public class CompanyServiceProvidersConfigurations : IEntityTypeConfiguration<CompanyServiceProvider>
    //{
    //    public void Configure(EntityTypeBuilder<CompanyServiceProvider> companyServiceProviderBuilder)
    //    {
    //    }
    //}

    // public class ServiceProvidersConfigurations : IEntityTypeConfiguration<ServiceProvider>
    // {
    //     public void Configure(EntityTypeBuilder<ServiceProvider> serviceProviderBuilder)
    //     {
    //         serviceProviderBuilder.HasMany(n => n.Companies);
    //     }
    // }
}
