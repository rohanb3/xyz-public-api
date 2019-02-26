using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xyzies.TWC.Public.Data.Entities.EntityConfigurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> branchBuilder)
        {
            branchBuilder.ToTable("TWC_Branches").HasKey(p => p.Id).HasName("BranchID");

            branchBuilder.Property(p => p.BranchName).HasMaxLength(250).IsRequired();
            branchBuilder.Property(p => p.Email).HasMaxLength(50);
            branchBuilder.Property(p => p.Phone).HasMaxLength(50);
            branchBuilder.Property(p => p.Fax).HasMaxLength(50);
            branchBuilder.Property(p => p.Address).HasMaxLength(50);
            branchBuilder.Property(p => p.City).HasMaxLength(50);
            branchBuilder.Property(p => p.ZipCode).HasMaxLength(50);
            branchBuilder.Property(p => p.GeoLat).HasMaxLength(50);
            branchBuilder.Property(p => p.GeoLon).HasMaxLength(50);
            branchBuilder.Property(p => p.State).HasMaxLength(50);
            branchBuilder.Property(p => p.CreatedDate).HasComputedColumnSql("GETUTCDATE()");
            branchBuilder.Property(p => p.ModifiedDate).HasComputedColumnSql("GETUTCDATE()").ValueGeneratedOnUpdate().Metadata.IsStoreGeneratedAlways = true;
            branchBuilder.Property(p => p.ParentCompanyId);

            branchBuilder.HasOne(n => n.ParentCompany);
        }
    }
}
