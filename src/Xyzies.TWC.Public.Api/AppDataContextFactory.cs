using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Xyzies.TWC.Public.Data;

namespace Xyzies.TWC.Public.Api
{
    /// <summary>
    /// Remove
    /// </summary>
    public class CablePortalAppDataContextFactory : IDesignTimeDbContextFactory<CablePortalAppDataContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CablePortalAppDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CablePortalAppDataContext>();
            optionsBuilder.UseSqlServer($"Data Source=173.82.28.90;Initial Catalog=TWC04052019;User ID=sa;Password=4@ndr3w.");
            //($"Data Source=173.82.28.90;Initial Catalog=TWC04052019;User ID=sa;Password=4@ndr3w.");

            return new CablePortalAppDataContext(optionsBuilder.Options);
        }
    }
}
