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
            optionsBuilder.UseSqlServer($"Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = TWC02122019; Integrated Security = True; Pooling = False");
                //($"Data Source=173.82.28.90;Initial Catalog=TWC04052019;User ID=sa;Password=4@ndr3w.");

            return new AppDataContext(optionsBuilder.Options);
        }
    }
}
