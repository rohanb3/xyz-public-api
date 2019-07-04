using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data.Entities;
using Data = Xyzies.TWC.Public.Data;

namespace Xyzies.TWC.Public.Api.Tests.IntegrationTests.Controllers
{
    public class BranchControllerTest : IClassFixture<BaseTest>
    {
        private readonly BaseTest _baseTest = null;
        private readonly string _baseBrqanchUrl = null;
        private readonly string TOKEN = Consts.StaticToken;

        public BranchControllerTest(BaseTest baseTest)
        {
            _baseTest = baseTest ?? throw new ArgumentNullException(nameof(baseTest));
            _baseTest.DbContext.ClearContext();

            _baseBrqanchUrl = "branch";
        }

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenGetBranches()
        {
            // Arrange

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.GetAsync(_baseBrqanchUrl);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByStateWhenGetBranches()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithOneState = 4;
            string state = "TestState";
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x=>x.Company, company).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x=>x.State, state)
                                                .With(x=>x.Company, company)
                                                .CreateMany(branchCountWithOneState));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            var uri = $"{_baseBrqanchUrl}?{nameof(BranchFilter.StateFilter)}={state}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(branchCountWithOneState);
            result.Data.Count.Should().Be(branchCountWithOneState);
            result.Data.All(x => x.State.ToLower().Contains(state.ToLower()));
        }

        //[Fact]
        //public async Task GetForInternalServices()
        //{
        //    //Arrange
        //    using (var http = _testServer.CreateClient())
        //    {
        //        http.BaseAddress = BASE_ADDRESS;
        //        Uri.TryCreate($"branch/{TOKEN}/trusted", UriKind.Relative, out Uri uri);

        //        //Act
        //        var response = await http.GetAsync(uri);

        //        //Assert
        //        response.EnsureSuccessStatusCode();
        //    }
        //}

    }
}
