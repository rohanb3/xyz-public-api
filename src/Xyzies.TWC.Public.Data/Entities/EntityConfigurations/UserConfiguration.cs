using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xyzies.TWC.Public.Data.Entities.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> salesBuilder)
        {
            salesBuilder.ToTable("TWC_Users").HasKey(p => p.Id).HasName("UserID");

            salesBuilder.Property(p => p.Name).HasMaxLength(250);
            salesBuilder.Property(p => p.Phone).HasMaxLength(50);
            salesBuilder.Property(p => p.Password).HasMaxLength(50);
            salesBuilder.Property(p => p.Email).HasMaxLength(50);
            salesBuilder.Property(p => p.Address).HasMaxLength(50);
            salesBuilder.Property(p => p.City).HasMaxLength(50);
            salesBuilder.Property(p => p.State).HasMaxLength(50);
            salesBuilder.Property(p => p.ZipCode).HasMaxLength(50);
            salesBuilder.Property(p => p.RoleId1).HasMaxLength(50);

        }
    }
}
