using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;
using Xyzies.TWC.Public.Api.Models;

namespace Xyzies.TWC.Public.Api.Tests.IntegrationTests.Controllers
{
    public class CompanyControllerTest : IClassFixture<BaseTest>
    {
        private readonly BaseTest _baseTest = null;
        private readonly string _baseCompanyUrl = null;

        public CompanyControllerTest(BaseTest baseTest)
        {
            _baseTest = baseTest ?? throw new ArgumentNullException(nameof(baseTest));
            _baseTest.DbContext.ClearContext();

            _baseCompanyUrl = "company";
        }


    }
}
