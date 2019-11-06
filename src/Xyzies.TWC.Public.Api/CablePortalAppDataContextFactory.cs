using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
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

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            string cpDbConnectionString = configuration.GetConnectionString("cpdb");
            optionsBuilder.UseSqlServer(cpDbConnectionString);
            return new CablePortalAppDataContext(optionsBuilder.Options);
        }
    }
}
