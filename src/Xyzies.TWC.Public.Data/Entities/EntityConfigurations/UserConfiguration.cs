using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xyzies.TWC.Public.Data.Entities.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> salesBuilder)
        {
            salesBuilder.ToTable("TWC_Users").HasKey(p => p.Id).HasName("UserID");
        }
    }
}
