//using Microsoft.AspNetCore.TestHost;
//using System;
//using System.Threading.Tasks;
//using Xunit;
//using Xyzies.TWC.Public.Api.Models;

//namespace Xyzies.TWC.Public.Api.Tests.IntegrationTests.Controllers
//{
//    public class CompanyControllerTest : IDisposable
//    {
//        private TestServer _testServer = null;
//        private readonly Uri BASE_ADDRESS = new Uri("http://localhost:8083");
//        private readonly string TOKEN = Consts.StaticToken;

//        public CompanyControllerTest()
//        {
//            _testServer = Startup.CreateTestServer() ?? throw new InvalidOperationException("Issues with test server");
//        }

//        [Fact]
//        public async Task GetForInternalServices()
//        {
//            //Arrange
//            using (var http = _testServer.CreateClient())
//            {
//                http.BaseAddress = BASE_ADDRESS;
//                Uri.TryCreate($"company/{TOKEN}/trusted", UriKind.Relative, out Uri uri);

//                //Act
//                var response = await http.GetAsync(uri);

//                //Assert
//                response.EnsureSuccessStatusCode();
//            }
//        }

//        public void Dispose()
//        {
//            _testServer.Dispose();
//        }

//    }
//}
