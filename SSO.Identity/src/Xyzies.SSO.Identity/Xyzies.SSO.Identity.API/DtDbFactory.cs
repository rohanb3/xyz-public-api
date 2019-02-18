using Xyzies.SSO.Identity.Data;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Xyzies.SSO.Identity.API
{

    public class IdentityDataContextFactory : IDesignTimeDbContextFactory<IdentityDataContext>
    {
        public IdentityDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityDataContext>();
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-MDU10E0;Initial Catalog=timewarner_20181026_test;Integrated Security=SSPI;");//User ID=sa;Password=secret123");

            return new IdentityDataContext(optionsBuilder.Options);
        }
    }
}
