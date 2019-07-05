using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Api.Tests.IntegrationTests.Services;
using Xyzies.TWC.Public.Api.Tests.Models.User;
using Xyzies.TWC.Public.Data;
using Xyzies.TWC.Public.Data.Entities;

namespace Xyzies.TWC.Public.Api.Tests
{
    public class BaseTest : IAsyncLifetime
    {
        private readonly object _lock = new object();
        public TestServer TestServer;
        public HttpClient HttpClient;
        public AppDataContext DbContext;
        public TestSeed TestSeed;
        public Fixture Fixture;
        public TokenModel AdminToken;
        public IHttpServiceTest HttpServiceTest = null;
        public User AdminProfile = null;
        public readonly int SalesRoleId = 2;
        private UserLoginOption _userLogin = null;

        public async Task DisposeAsync()
        {
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

                var dbConnectionString = configuration.GetConnectionString("db");

                if (string.IsNullOrEmpty(dbConnectionString))
                {
                    throw new ApplicationException("Missing the connection string to database");
                };
                webHostBuild.ConfigureServices(service =>
                {
                    service.AddDbContextPool<AppDataContext>(ctxOptions =>
                    {
                        ctxOptions.UseInMemoryDatabase(dbConnectionString).EnableSensitiveDataLogging();
                        ctxOptions.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    });
                    service.Configure<UserLoginOption>(options => configuration.Bind("TestUserCredential", options));
                    service.AddScoped<IHttpServiceTest, HttpServiceTest>();
                });
                TestServer = new TestServer(webHostBuild);
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
