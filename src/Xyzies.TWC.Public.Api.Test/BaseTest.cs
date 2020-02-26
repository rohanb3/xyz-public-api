using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Api.Tests.IntegrationTests.Services;
using Xyzies.TWC.Public.Api.Tests.Models.User;
using Xyzies.TWC.Public.Data;

namespace Xyzies.TWC.Public.Api.Tests
{
    public class BaseTest : IAsyncLifetime
    {
        private readonly object _lock = new object();
        public TestServer TestServer;
        public HttpClient HttpClient;
        public CablePortalAppDataContext CableDbContext;
        public AppDataContext DbContext;
        private CloudBlobContainer _cloudBlobContainer;
        public TestSeed TestSeed;
        public Fixture Fixture;
        public TokenModel AdminToken;
        public IHttpServiceTest HttpServiceTest = null;
        public User AdminProfile = null;
        public readonly int SalesRoleId = 2;
        public readonly int DefaultCompanyId = 145;
        private UserLoginOption _userLogin = null;

        public async Task DisposeAsync()
        {
            await DeleteImageInBlobStorage();
            CableDbContext.Dispose();
            DbContext.Dispose();
            HttpClient.Dispose();
            TestServer.Dispose();
        }

        public async Task InitializeAsync()
        {
            lock (_lock)
            {
                var builder = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true);

                var configuration = builder.Build();

                IWebHostBuilder webHostBuild =
                        WebHost.CreateDefaultBuilder()
                               .UseStartup<TestStartUp>()
                               .UseWebRoot(Directory.GetCurrentDirectory())
                               .UseContentRoot(Directory.GetCurrentDirectory());

                var cableDbConnectionString = configuration.GetConnectionString("cpdb");
                var dbConnectionString = configuration.GetConnectionString("db");

                if (string.IsNullOrWhiteSpace(cableDbConnectionString) || string.IsNullOrWhiteSpace(dbConnectionString))
                {
                    throw new ApplicationException("Missing the connection string to database");
                };
                webHostBuild.ConfigureServices(service =>
                {
                    service.AddDbContextPool<CablePortalAppDataContext>(ctxOptions =>
                    {
                        ctxOptions.UseInMemoryDatabase(cableDbConnectionString).EnableSensitiveDataLogging();
                        ctxOptions.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    });
                    service.AddDbContextPool<AppDataContext>(ctxOptions =>
                    {
                        ctxOptions.UseInMemoryDatabase(dbConnectionString).EnableSensitiveDataLogging();
                        ctxOptions.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    });
                    service.Configure<UserLoginOption>(options => configuration.Bind("TestUserCredential", options));
                    service.AddScoped<IHttpServiceTest, HttpServiceTest>();
                    string azureStorageConnectionString = configuration["connectionStrings:storageAccount"];

                    if (!CloudStorageAccount.TryParse(azureStorageConnectionString, out CloudStorageAccount storageAccount))
                    {
                        throw new ApplicationException("Missing Azure Blob Storage settings");
                    }
                    var blobStorageClient = storageAccount.CreateCloudBlobClient();
                    _cloudBlobContainer = blobStorageClient.GetContainerReference(DefaultCompanyId.ToString());
                    if (_cloudBlobContainer == null)
                    {
                        throw new ApplicationException("Blob container not exist");
                    }
                });
                TestServer = new TestServer(webHostBuild);
                CableDbContext = TestServer.Host.Services.GetRequiredService<CablePortalAppDataContext>();
                DbContext = TestServer.Host.Services.GetRequiredService<AppDataContext>();
                TestSeed = TestServer.Host.Services.GetRequiredService<TestSeed>();
                HttpClient = TestServer.CreateClient();
                Fixture = new Fixture();
                Fixture.Customizations.Add(new IgnoreVirtualMembers());

                _userLogin = TestServer.Host.Services.GetRequiredService<IOptions<UserLoginOption>>()?.Value;
                HttpServiceTest = TestServer.Host.Services.GetRequiredService<IHttpServiceTest>();
            }
            AdminToken = await HttpServiceTest.GetAuthorizationToken(_userLogin);
            AdminProfile = await HttpServiceTest.GetUserProfile(AdminToken);
        }

        public async Task AddImageInBlobStorage(IFormFile file)
        {
            try
            {
                var fileName = $"avatar.{DefaultCompanyId.ToString()}{Path.GetExtension(file.FileName)}";

                CloudBlockBlob cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName);
                await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());
            }
            catch (Exception)
            {
                throw new ApplicationException("Add file");
            }
        }

        public async Task DeleteImageInBlobStorage(string extension = null)
        {
            try
            {
                List<string> fileNamesList = new List<string>();
                CloudBlockBlob cloudBlockBlob = null;

                if (extension != null)
                {
                    fileNamesList.Add($"avatar.{DefaultCompanyId.ToString()}{extension}");
                }
                else
                {
                    var blobItem = await _cloudBlobContainer.ListBlobsSegmentedAsync(null);
                    if (blobItem != null)
                    {
                        fileNamesList.AddRange(blobItem.Results.Select(x => Path.GetFileName(x.Uri.AbsolutePath)));
                    }
                }

                foreach (var fileName in fileNamesList)
                {
                    cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName);
                    if (!(await cloudBlockBlob.DeleteIfExistsAsync()))
                    {
                        throw new ApplicationException();
                    }
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Delete file");
            }
        }
    }


    public class IgnoreVirtualMembers : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var pi = request as PropertyInfo;
            if (pi == null)
            {
                return new NoSpecimen();
            }

            if (pi.GetGetMethod().IsVirtual)
            {
                return null;
            }
            return new NoSpecimen();
        }
    }
}
