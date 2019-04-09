using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xyzies.TWC.Public.Data.Entities.EntityConfigurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> companyBuilder)
        {
            companyBuilder.ToTable("TWC_Companies").HasKey(p => p.Id).HasName("CompanyID");

            companyBuilder.Property(p => p.IsEnabled).HasDefaultValue(true);
        }
    }
}
