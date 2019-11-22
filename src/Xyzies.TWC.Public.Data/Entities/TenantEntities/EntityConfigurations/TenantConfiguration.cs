using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Xyzies.TWC.Public.Data.Entities.TenantEntities.EntityConfigurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public TenantConfiguration() { }
        public void Configure(EntityTypeBuilder<Tenant> tenantBuilder)
        {
            tenantBuilder.HasData(new Tenant[]
                {
                    new Tenant
                    {
                        Id = Guid.Parse("0ed21401-e0e6-4b22-aa89-4c5522212b67"),
                        TenantName = "Spectrum",
                        Phone = "380938821599",
                        CreatedOn = DateTime.UtcNow
                    }
                });
        }
    }
}
