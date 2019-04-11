using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xyzies.TWC.Public.Data.Entities.EntityConfigurations
{
    public class BranchContactTypeConfiguration : IEntityTypeConfiguration<BranchContactType>
    {
        public void Configure(EntityTypeBuilder<BranchContactType> branchContactTypeBuilder)
        {
            branchContactTypeBuilder.ToTable("TWC_BranchContactType").HasKey(p => p.Id);

            branchContactTypeBuilder.Property(p => p.Name).HasMaxLength(128);
        }
    }
}
