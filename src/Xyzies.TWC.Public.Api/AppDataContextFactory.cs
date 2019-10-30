using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Xyzies.TWC.Public.Data;

namespace Xyzies.TWC.Public.Api
{
    /// <summary>
    /// Remove
    /// </summary>
    public class AppDataContextFactory : IDesignTimeDbContextFactory<AppDataContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public AppDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDataContext>();
            optionsBuilder.UseSqlServer($"Server=tcp:xyzies-db-dev.database.windows.net,1433;Initial Catalog=xyzies-public-api-dev;Persist Security Info=False;User ID=xyzies;Password=kl91jd2f2zLk3Ndf2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            //($"Data Source=173.82.28.90;Initial Catalog=TWC04052019;User ID=sa;Password=4@ndr3w.");

            return new AppDataContext(optionsBuilder.Options);
        }
    }
}
