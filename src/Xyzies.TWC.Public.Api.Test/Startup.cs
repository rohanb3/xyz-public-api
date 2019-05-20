using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using System;
using Xyzies.TWC.Public.Api.Managers;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Data;
using Xyzies.TWC.Public.Data.Repositories;
using Xyzies.TWC.Public.Data.Repositories.Azure;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api.Tests
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder();
                //.SetBasePath(env.ContentRootPath);
                //.AddEnvironmentVariables();

            Configuration = builder.Build();

            //env.Initialize(env.ApplicationName, env.ContentRootPath, new WebHostOptions
            //{
            //    Environment = "test"
            //});
        }

        public static string ServiceBaseUrlPrefix { get; set; } = "/api/public-api/";

        /// <summary>
        /// Configuration container
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            string dbConnectionString = $"Data Source=173.82.28.90;Initial Catalog=TWC04242019;User ID=sa;Password=4@ndr3w.";

            string azureStorageConnectionString = $"DefaultEndpointsProtocol=https;AccountName=companyavatarsblobdev;AccountKey=9ZhcHZXumof76ruRZf2SPCpIOgWdVLpdCrfu1x2Gfz5Ve9k/yfVtJTEd66rYx7bZ3LOiKBVQ2hLaYHDVFRuFHw==;EndpointSuffix=core.windows.net";
            if (!CloudStorageAccount.TryParse(azureStorageConnectionString, out CloudStorageAccount storageAccount))
            {
                throw new ApplicationException("Missing Azure Blob Storage settings");
            }

            services.AddSingleton(storageAccount);

            services.AddDbContext<AppDataContext>(ctxOptions =>
               ctxOptions.UseSqlServer(dbConnectionString));

            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped <IAzureCompanyAvatarRepository, AzureCompanyAvatarRepository>();
            services.AddScoped<ICompanyAvatarsManager, CompanyAvatarsManager>();

            services.AddScoped<IBranchManager, BranchManager>();


            services.AddMvc(options => options.
            EnableEndpointRouting = true).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc();
        }

        public static TestServer CreateTestServer() =>
            new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
    }
}
