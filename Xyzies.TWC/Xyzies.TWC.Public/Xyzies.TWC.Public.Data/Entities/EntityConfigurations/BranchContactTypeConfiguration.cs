using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xyzies.TWC.Public.Data.Entities.EntityConfigurations
{
    public class BranchContactTypeConfiguration : IEntityTypeConfiguration<BranchContactType>
    {
        public void Configure(EntityTypeBuilder<BranchContactType> branchContactTypeBuilder)
        {
            branchContactTypeBuilder.ToTable("TWC_BranchContactTypes").HasKey(p => p.Id).HasName("BranchContactTypeID");

            branchContactTypeBuilder.Property(p => p.Name).HasMaxLength(50);
        }
    }
}
