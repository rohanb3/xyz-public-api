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
    public class AppDataContextFactory : IDesignTimeDbContextFactory<AppDataContext>
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public AppDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDataContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();
            var connectionString = configuration.GetConnectionString("db");
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDataContext(optionsBuilder.Options);
        }
    }
}
