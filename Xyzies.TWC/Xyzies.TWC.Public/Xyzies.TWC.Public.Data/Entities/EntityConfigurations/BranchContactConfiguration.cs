using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xyzies.TWC.Public.Data.Entities.EntityConfigurations
{
    public class BranchContactConfiguration : IEntityTypeConfiguration<BranchContact>
    {
        public void Configure(EntityTypeBuilder<BranchContact> branchContactBuilder)
        {
            branchContactBuilder.ToTable("TWC_BranchContacts").HasKey(p => p.Id).HasName("BranchContactID");

            branchContactBuilder.Property(p => p.PersonName).HasMaxLength(50);
            branchContactBuilder.Property(p => p.PersonLastName).HasMaxLength(50);
            branchContactBuilder.Property(p => p.PersonTitle).HasMaxLength(100);
            branchContactBuilder.Property(p => p.Value).HasMaxLength(100).IsRequired();
            branchContactBuilder.Property(p => p.CreatedDate).HasComputedColumnSql("GETUTCDATE()").ValueGeneratedOnAdd();
            branchContactBuilder.Property(p => p.ModifiedDate).HasComputedColumnSql("GETUTCDATE()").ValueGeneratedOnUpdate().Metadata.IsStoreGeneratedAlways = true; ;

            branchContactBuilder
            .HasOne(n => n.BranchContactType)
            .WithMany(c => c.BranchContacts)
            .HasForeignKey(sc => sc.BranchContactTypeId);

            branchContactBuilder
            .HasOne(n => n.BranchPrimaryContact)
            .WithMany(c => c.BranchContacts)
            .HasForeignKey(sc => sc.BranchPrimaryContactId);
        }
    }
}
