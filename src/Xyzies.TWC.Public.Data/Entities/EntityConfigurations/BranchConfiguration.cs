using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xyzies.TWC.Public.Data.Entities.EntityConfigurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> branchBuilder)
        {
            branchBuilder.ToTable("TWC_Branches").HasKey(p => p.Id).HasName("BranchID");
            
            branchBuilder.Property(p => p.BranchName).HasMaxLength(256).IsRequired();
            branchBuilder.Property(p => p.Email).HasMaxLength(128);
            branchBuilder.Property(p => p.Phone).HasMaxLength(16);
            branchBuilder.Property(p => p.Fax).HasMaxLength(16);
            branchBuilder.Property(p => p.AddressLine1);
            branchBuilder.Property(p => p.AddressLine2);
            branchBuilder.Property(p => p.City).HasMaxLength(64);
            branchBuilder.Property(p => p.ZipCode).HasMaxLength(16);
            branchBuilder.Property(p => p.GeoLat).HasMaxLength(32);
            branchBuilder.Property(p => p.GeoLng).HasMaxLength(32);
            branchBuilder.Property(p => p.State).HasMaxLength(64);
            branchBuilder.Property(p => p.IsEnabled).HasDefaultValue(true);

            branchBuilder.HasOne(n => n.Company);
        }
    }
}
