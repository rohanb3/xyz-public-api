using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Api.Tests.Extensions;
using Xyzies.TWC.Public.Data.Entities;
using Data = Xyzies.TWC.Public.Data;

namespace Xyzies.TWC.Public.Api.Tests.IntegrationTests.Controllers
{
    public class BranchControllerTest : IClassFixture<BaseTest>
    {
        private readonly BaseTest _baseTest = null;
        private readonly string _baseBrqanchUrl = null;

        public BranchControllerTest(BaseTest baseTest)
        {
            _baseTest = baseTest ?? throw new ArgumentNullException(nameof(baseTest));
            _baseTest.DbContext.ClearContext();
            _baseTest.TestSeed.Seed();
            _baseBrqanchUrl = "branch";
        }

        #region Get branches list

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
            string state = _baseTest.Fixture.Create<string>();
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
            result.Data.All(x => x.State.ToLower() == state.ToLower()).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByCityWhenGetBranches()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithOneCity = 4;
            string city = _baseTest.Fixture.Create<string>();
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x => x.City, city)
                                                .With(x => x.Company, company)
                                                .CreateMany(branchCountWithOneCity));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            var uri = $"{_baseBrqanchUrl}?{nameof(BranchFilter.CityFilter)}={city}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(branchCountWithOneCity);
            result.Data.Count.Should().Be(branchCountWithOneCity);
            result.Data.All(x => x.City.ToLower() == city.ToLower()).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByEmailWhenGetBranches()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithOneCity = 4;
            string email = _baseTest.Fixture.Create<string>();
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x => x.Email, email)
                                                .With(x => x.Company, company)
                                                .CreateMany(branchCountWithOneCity));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            var uri = $"{_baseBrqanchUrl}?{nameof(BranchFilter.EmailFilter)}={email}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(branchCountWithOneCity);
            result.Data.Count.Should().Be(branchCountWithOneCity);
            result.Data.All(x => x.Email.ToLower() == email.ToLower()).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByBranchNameWhenGetBranches()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithOneCity = 4;
            string branchName = _baseTest.Fixture.Create<string>();
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x => x.BranchName, branchName)
                                                .With(x => x.Company, company)
                                                .CreateMany(branchCountWithOneCity));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            var uri = $"{_baseBrqanchUrl}?{nameof(BranchFilter.BranchNameFilter)}={branchName}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(branchCountWithOneCity);
            result.Data.Count.Should().Be(branchCountWithOneCity);
            result.Data.All(x => x.BranchName.ToLower() == branchName.ToLower()).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByEnabledWhenGetBranches()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithOneCity = 4;
            bool isEnabled = true;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            _baseTest.DbContext.Branches.ToList().ForEach(x => x.IsEnabled = !isEnabled);
            var branches = _baseTest.Fixture.Build<Branch>()
                                            .With(x => x.IsEnabled, !isEnabled)
                                            .With(x => x.Company, company).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x => x.IsEnabled, isEnabled)
                                                .With(x => x.Company, company)
                                                .CreateMany(branchCountWithOneCity));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            var uri = $"{_baseBrqanchUrl}?{nameof(BranchFilter.IsEnabledFilter)}={isEnabled}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(branchCountWithOneCity);
            result.Data.Count.Should().Be(branchCountWithOneCity);
            result.Data.All(x => x.IsEnabled).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByDisabledWhenGetBranches()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithOneCity = 4;
            bool isEnabled = false;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            _baseTest.DbContext.Branches.ToList().ForEach(x => x.IsEnabled = !isEnabled);
            var branches = _baseTest.Fixture.Build<Branch>()
                                            .With(x=>x.IsEnabled, !isEnabled)
                                            .With(x => x.Company, company).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x => x.IsEnabled, isEnabled)
                                                .With(x => x.Company, company)
                                                .CreateMany(branchCountWithOneCity));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            var uri = $"{_baseBrqanchUrl}?{nameof(BranchFilter.IsEnabledFilter)}={isEnabled}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(branchCountWithOneCity);
            result.Data.Count.Should().Be(branchCountWithOneCity);
            result.Data.All(x => !x.IsEnabled).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByBranchIdWhenGetBranches()
        {
            // Arrange
            int branchCount = 10;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();

            var expectedBranch = _baseTest.DbContext.Branches.First();
            var uri = $"{_baseBrqanchUrl}?{nameof(BranchFilter.BranchIdFilter)}={expectedBranch.Id}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(1);
            result.Data.Count.Should().Be(1);
            result.Data.First().Id.Should().Be(expectedBranch.Id);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByBranchIdWithIgnoreOtherFiltersListWhenGetBranches()
        {
            // Arrange
            int branchCount = 10;
            string state = _baseTest.Fixture.Create<string>();
            string city = _baseTest.Fixture.Create<string>();
            string email = _baseTest.Fixture.Create<string>();
            string branchName = _baseTest.Fixture.Create<string>();
            bool isEnabled = true;

            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>()
                                            .With(x=>x.State, state)
                                            .With(x=>x.City, city)
                                            .With(x => x.Email, email)
                                            .With(x => x.BranchName, branchName)
                                            .With(x => x.IsEnabled, isEnabled)
                                            .With(x => x.Company, company).CreateMany(branchCount).ToList();
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();

            var expectedBranches = _baseTest.DbContext.Branches.Take(5).ToList();
            var expectedBranchesIdsQuery = string.Join('&', expectedBranches.Select(x => $"{nameof(BranchFilter.BranchIds)}={x.Id}"));
            var uri = $"{_baseBrqanchUrl}?{nameof(BranchFilter.StateFilter)}={state}&{nameof(BranchFilter.CityFilter)}={city}&{nameof(BranchFilter.EmailFilter)}={email}&{nameof(BranchFilter.BranchNameFilter)}={branchName}&{nameof(BranchFilter.IsEnabledFilter)}={isEnabled}&{expectedBranchesIdsQuery}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(expectedBranches.Count());
            result.Data.Count.Should().Be(expectedBranches.Count());
            result.Data.All(x=>expectedBranches.Select(u=>u.Id).Contains(x.Id)).Should().BeTrue();
        }

        [Theory]
        [InlineData(0, 50)]
        [InlineData(30, 35)]
        public async Task ShouldReturnSuccessResultWithPaginationWhenGetBranches(int skip, int take)
        {
            // Arrange
            int branchCount = 100;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();

            var uri = $"{_baseBrqanchUrl}?{nameof(Paginable.Skip)}={skip}&{nameof(Paginable.Take)}={take}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(_baseTest.DbContext.Branches.Count());
            result.Data.Count.Should().Be(_baseTest.DbContext.Branches.Skip(skip).Take(take).Count());
            result.ItemsPerPage.Should().Be(take);
        }

        [Theory]
        //------ Default sorting
        //[InlineData(nameof(BranchModel.CreatedDate), "", "")]
        //-----------------------

        [InlineData(nameof(BranchModel.CreatedDate), "createddate", "desc")]
        [InlineData(nameof(BranchModel.CreatedDate), "createddate", "asc")]

        [InlineData(nameof(BranchModel.IsEnabled), "status", "desc")]
        [InlineData(nameof(BranchModel.IsEnabled), "status", "asc")]

        [InlineData(nameof(BranchModel.State), "state", "desc")]
        [InlineData(nameof(BranchModel.State), "state", "asc")]

        [InlineData(nameof(BranchModel.City), "city", "desc")]
        [InlineData(nameof(BranchModel.City), "city", "asc")]

        [InlineData(nameof(BranchModel.BranchName), "branchname", "desc")]
        [InlineData(nameof(BranchModel.BranchName), "branchname", "asc")]

        [InlineData(nameof(BranchModel.Id), "id", "desc")]
        [InlineData(nameof(BranchModel.Id), "id", "asc")]

        /// TODO is valid case
        [InlineData(nameof(BranchModel.IsEnabled), "isenabled", "desc")]
        [InlineData(nameof(BranchModel.IsEnabled), "isenabled", "asc")]
        public async Task ShouldReturnSuccessResultWithSortingWhenGetBranches(string sortBy, string sortByRequest, string order)
        {
            // Arrange
            int branchCount = 100;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();

            var uri = $"{_baseBrqanchUrl}?{nameof(Sortable.SortBy)}={sortByRequest}&{nameof(Sortable.SortOrder)}={order}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);

            //Assert
            var expression = typeof(BranchModel).GetExpression<BranchModel>(sortBy);

            result.Total.Should().Be(_baseTest.DbContext.Branches.Count());
            result.Data.Count.Should().Be(_baseTest.DbContext.Branches.Count());
            result.Data.All(x => x.GetType().GetProperty(sortBy).GetValue(x) == null).Should().BeFalse();
            result.ItemsPerPage.Should().Be(0);
            if (order == "asc")
            {
                bool resultSequence = result.Data.SequenceEqual(result.Data.OrderBy(expression));
                resultSequence.Should().BeTrue();
            }
            else
            {
                bool resultSequence = result.Data.SequenceEqual(result.Data.OrderByDescending(expression));
                resultSequence.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWithCorrectDataWhenGetBranches()
        {
            // Arrange
            int usersCount = 10;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branchId = Guid.NewGuid();
            var branch = _baseTest.Fixture.Build<Branch>()
                                            .With(x=>x.Id, branchId)
                                            .With(x => x.Company, company)
                                            .Create();
            var users = _baseTest.Fixture.Build<Users>()
                                         .With(x => x.Role, _baseTest.SalesRoleId.ToString())
                                         .With(x => x.BranchId, branchId)
                                         .CreateMany(usersCount);
            _baseTest.DbContext.Branches.Add(branch);
            _baseTest.DbContext.Users.AddRange(users);
            _baseTest.DbContext.SaveChanges();

            var uri = $"{_baseBrqanchUrl}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);

            //Assert
            result.Total.Should().Be(_baseTest.DbContext.Branches.Count());
            result.Data.Count.Should().Be(_baseTest.DbContext.Branches.Count());
            result.ItemsPerPage.Should().Be(0);

            var branchModel = result.Data.First(x => x.Id == branch.Id);
            branchModel.BranchName.Should().Be(branch.BranchName);
            branchModel.CountSalesRep.Should().Be(usersCount);
            branchModel.Email.Should().Be(branch.Email);
            branchModel.Phone.Should().Be(branch.Phone);
            branchModel.Fax.Should().Be(branch.Fax);
            branchModel.AddressLine1.Should().Be(branch.AddressLine1);
            branchModel.AddressLine2.Should().Be(branch.AddressLine2);
            branchModel.City.Should().Be(branch.City);
            branchModel.ZipCode.Should().Be(branch.ZipCode);
            branchModel.GeoLat.Should().Be(branch.GeoLat);
            branchModel.GeoLng.Should().Be(branch.GeoLng);
            branchModel.State.Should().Be(branch.State);
            branchModel.IsEnabled.Should().Be(branch.IsEnabled);
            branchModel.CreatedDate.Should().Be(branch.CreatedDate);
            branchModel.ModifiedDate.Should().Be(branch.ModifiedDate);
            branchModel.CreatedBy.Should().Be(branch.CreatedBy);
            branchModel.ModifiedBy.Should().Be(branch.ModifiedBy);
            branchModel.CompanyId.Should().Be(branch.CompanyId);
        }

        #endregion;

        #region Get Branch list by trusted token

        [Fact]
        public async Task ShouldReturnForbidResultWhenGetBranchesByTrustedToken()
        {
            // Arrange
            string uri = $"{_baseBrqanchUrl}/{Guid.NewGuid()}/trusted";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWhenGetBranchesByTrustedToken()
        {
            // Arrange
            int usersCount = 10;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branchId = Guid.NewGuid();
            var branch = _baseTest.Fixture.Build<Branch>()
                                            .With(x => x.Id, branchId)
                                            .With(x => x.Company, company)
                                            .Create();
            var users = _baseTest.Fixture.Build<Users>()
                                         .With(x => x.Role, _baseTest.SalesRoleId.ToString())
                                         .With(x => x.BranchId, branchId)
                                         .CreateMany(usersCount);
            _baseTest.DbContext.Branches.Add(branch);
            _baseTest.DbContext.Users.AddRange(users);
            _baseTest.DbContext.SaveChanges();

            string uri = $"{_baseBrqanchUrl}/{Consts.StaticToken}/trusted";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Total.Should().Be(_baseTest.DbContext.Branches.Count());
            result.Data.Count.Should().Be(_baseTest.DbContext.Branches.Count());
            result.ItemsPerPage.Should().Be(0);

            var branchModel = result.Data.First(x => x.Id == branch.Id);
            branchModel.BranchName.Should().Be(branch.BranchName);
            branchModel.CountSalesRep.Should().Be(usersCount);
            branchModel.Email.Should().Be(branch.Email);
            branchModel.Phone.Should().Be(branch.Phone);
            branchModel.Fax.Should().Be(branch.Fax);
            branchModel.AddressLine1.Should().Be(branch.AddressLine1);
            branchModel.AddressLine2.Should().Be(branch.AddressLine2);
            branchModel.City.Should().Be(branch.City);
            branchModel.ZipCode.Should().Be(branch.ZipCode);
            branchModel.GeoLat.Should().Be(branch.GeoLat);
            branchModel.GeoLng.Should().Be(branch.GeoLng);
            branchModel.State.Should().Be(branch.State);
            branchModel.IsEnabled.Should().Be(branch.IsEnabled);
            branchModel.CreatedDate.Should().Be(branch.CreatedDate);
            branchModel.ModifiedDate.Should().Be(branch.ModifiedDate);
            branchModel.CreatedBy.Should().Be(branch.CreatedBy);
            branchModel.ModifiedBy.Should().Be(branch.ModifiedBy);
            branchModel.CompanyId.Should().Be(branch.CompanyId);
        }

        #endregion


        #region Get branch by id

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenGetBrancheById()
        {
            // Arrange
            string uri = $"{_baseBrqanchUrl}/{Guid.NewGuid()}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultWhenGetBrancheById()
        {
            // Arrange
            string uri = $"{_baseBrqanchUrl}/{Guid.NewGuid()}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task ShouldReturnSuccessdResultWhenGetBrancheById()
        {
            // Arrange
            int usersCount = 10;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branchId = Guid.NewGuid();
            var branch = _baseTest.Fixture.Build<Branch>()
                                            .With(x => x.Id, branchId)
                                            .With(x => x.Company, company)
                                            .Create();
            var users = _baseTest.Fixture.Build<Users>()
                                         .With(x => x.Role, _baseTest.SalesRoleId.ToString())
                                         .With(x => x.BranchId, branchId)
                                         .CreateMany(usersCount);
            _baseTest.DbContext.Branches.Add(branch);
            _baseTest.DbContext.Users.AddRange(users);
            _baseTest.DbContext.SaveChanges();
            string uri = $"{_baseBrqanchUrl}/{branchId}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var branchModel = JsonConvert.DeserializeObject<BranchModel>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            branchModel.BranchName.Should().Be(branch.BranchName);
            branchModel.CountSalesRep.Should().Be(usersCount);
            branchModel.Email.Should().Be(branch.Email);
            branchModel.Phone.Should().Be(branch.Phone);
            branchModel.Fax.Should().Be(branch.Fax);
            branchModel.AddressLine1.Should().Be(branch.AddressLine1);
            branchModel.AddressLine2.Should().Be(branch.AddressLine2);
            branchModel.City.Should().Be(branch.City);
            branchModel.ZipCode.Should().Be(branch.ZipCode);
            branchModel.GeoLat.Should().Be(branch.GeoLat);
            branchModel.GeoLng.Should().Be(branch.GeoLng);
            branchModel.State.Should().Be(branch.State);
            branchModel.IsEnabled.Should().Be(branch.IsEnabled);
            branchModel.CreatedDate.Should().Be(branch.CreatedDate);
            branchModel.ModifiedDate.Should().Be(branch.ModifiedDate);
            branchModel.CreatedBy.Should().Be(branch.CreatedBy);
            branchModel.ModifiedBy.Should().Be(branch.ModifiedBy);
            branchModel.CompanyId.Should().Be(branch.CompanyId);
        }

        #endregion

        #region GetBranchesByCompany

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenGetBranchesByCompany()
        {
            // Arrange
            int companyId = _baseTest.Fixture.Create<int>();
            string uri = $"company/{companyId}/{_baseBrqanchUrl}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultWhenGetBranchesByCompany()
        {
            // Arrange
            int companyId = _baseTest.Fixture.Create<int>();
            string uri = $"company/{companyId}/{_baseBrqanchUrl}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByCompanyWhenGetBranchesByCompany()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithFindCompany = 4;
            string state = _baseTest.Fixture.Create<string>();
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var firstCompany = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var secondCompany = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, firstCompany).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x => x.Company, secondCompany)
                                                .CreateMany(branchCountWithFindCompany));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            string uri = $"company/{secondCompany.Id}/{_baseBrqanchUrl}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(branchCountWithFindCompany);
            result.Data.Count.Should().Be(branchCountWithFindCompany);
            result.Data.All(x => x.CompanyId == secondCompany.Id).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByStateWhenGetBranchesByCompany()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithOneState = 4;
            string state = _baseTest.Fixture.Create<string>();
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x => x.State, state)
                                                .With(x => x.Company, company)
                                                .CreateMany(branchCountWithOneState));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            string uri = $"company/{company.Id}/{_baseBrqanchUrl}?{nameof(BranchFilter.StateFilter)}={state}";

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
            result.Data.All(x => x.State.ToLower() == state.ToLower()).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByCityWhenGetBranchesByCompany()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithOneCity = 4;
            string city = _baseTest.Fixture.Create<string>();
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x => x.City, city)
                                                .With(x => x.Company, company)
                                                .CreateMany(branchCountWithOneCity));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            var uri = $"company/{company.Id}/{_baseBrqanchUrl}?{nameof(BranchFilter.CityFilter)}={city}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(branchCountWithOneCity);
            result.Data.Count.Should().Be(branchCountWithOneCity);
            result.Data.All(x => x.City.ToLower() == city.ToLower()).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByEmailWhenGetBranchesByCompany()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithOneCity = 4;
            string email = _baseTest.Fixture.Create<string>();
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x => x.Email, email)
                                                .With(x => x.Company, company)
                                                .CreateMany(branchCountWithOneCity));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            var uri = $"company/{company.Id}/{_baseBrqanchUrl}?{nameof(BranchFilter.EmailFilter)}={email}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(branchCountWithOneCity);
            result.Data.Count.Should().Be(branchCountWithOneCity);
            result.Data.All(x => x.Email.ToLower() == email.ToLower()).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByBranchNameWhenGetBranchesByCompany()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithOneCity = 4;
            string branchName = _baseTest.Fixture.Create<string>();
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x => x.BranchName, branchName)
                                                .With(x => x.Company, company)
                                                .CreateMany(branchCountWithOneCity));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            var uri = $"company/{company.Id}/{_baseBrqanchUrl}?{nameof(BranchFilter.BranchNameFilter)}={branchName}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(branchCountWithOneCity);
            result.Data.Count.Should().Be(branchCountWithOneCity);
            result.Data.All(x => x.BranchName.ToLower() == branchName.ToLower()).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByEnabledWhenGetBranchesByCompany()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithOneCity = 4;
            bool isEnabled = true;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            _baseTest.DbContext.Branches.ToList().ForEach(x => x.IsEnabled = !isEnabled);
            var branches = _baseTest.Fixture.Build<Branch>()
                                            .With(x => x.IsEnabled, !isEnabled)
                                            .With(x => x.Company, company).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x => x.IsEnabled, isEnabled)
                                                .With(x => x.Company, company)
                                                .CreateMany(branchCountWithOneCity));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            var uri = $"company/{company.Id}/{_baseBrqanchUrl}?{nameof(BranchFilter.IsEnabledFilter)}={isEnabled}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(branchCountWithOneCity);
            result.Data.Count.Should().Be(branchCountWithOneCity);
            result.Data.All(x => x.IsEnabled).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByDisabledWhenGetBranchesByCompany()
        {
            // Arrange
            int branchCount = 10;
            int branchCountWithOneCity = 4;
            bool isEnabled = false;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            _baseTest.DbContext.Branches.ToList().ForEach(x => x.IsEnabled = !isEnabled);
            var branches = _baseTest.Fixture.Build<Branch>()
                                            .With(x => x.IsEnabled, !isEnabled)
                                            .With(x => x.Company, company).CreateMany(branchCount).ToList();
            branches.AddRange(_baseTest.Fixture.Build<Branch>()
                                                .With(x => x.IsEnabled, isEnabled)
                                                .With(x => x.Company, company)
                                                .CreateMany(branchCountWithOneCity));
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();
            var uri = $"company/{company.Id}/{_baseBrqanchUrl}?{nameof(BranchFilter.IsEnabledFilter)}={isEnabled}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(branchCountWithOneCity);
            result.Data.Count.Should().Be(branchCountWithOneCity);
            result.Data.All(x => !x.IsEnabled).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByBranchIdWhenGetBranchesByCompany()
        {
            // Arrange
            int branchCount = 10;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();

            var expectedBranch = _baseTest.DbContext.Branches.First();
            var uri = $"company/{company.Id}/{_baseBrqanchUrl}?{nameof(BranchFilter.BranchIdFilter)}={expectedBranch.Id}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(1);
            result.Data.Count.Should().Be(1);
            result.Data.First().Id.Should().Be(expectedBranch.Id);
        }

        /// TODO: Not working filter by branch ids list
        //[Fact]
        //public async Task ShouldReturnSuccessResultFileredByBranchIdWithIgnoreOtherFiltersListWhenGetBranchesByCompany()
        //{
        //    // Arrange
        //    int branchCount = 10;
        //    string state = _baseTest.Fixture.Create<string>();
        //    string city = _baseTest.Fixture.Create<string>();
        //    string email = _baseTest.Fixture.Create<string>();
        //    string branchName = _baseTest.Fixture.Create<string>();
        //    bool isEnabled = true;

        //    var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
        //    var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
        //    var branches = _baseTest.Fixture.Build<Branch>()
        //                                    .With(x => x.State, state)
        //                                    .With(x => x.City, city)
        //                                    .With(x => x.Email, email)
        //                                    .With(x => x.BranchName, branchName)
        //                                    .With(x => x.IsEnabled, isEnabled)
        //                                    .With(x => x.Company, company).CreateMany(branchCount).ToList();
        //    _baseTest.DbContext.Branches.AddRange(branches);
        //    _baseTest.DbContext.SaveChanges();

        //    var expectedBranches = _baseTest.DbContext.Branches.Take(5).ToList();
        //    var expectedBranchesIdsQuery = string.Join('&', expectedBranches.Select(x => $"{nameof(BranchFilter.BranchIds)}={x.Id}"));
        //    var uri = $"company/{company.Id}/{_baseBrqanchUrl}?{nameof(BranchFilter.StateFilter)}={state}&{nameof(BranchFilter.CityFilter)}={city}&{nameof(BranchFilter.EmailFilter)}={email}&{nameof(BranchFilter.BranchNameFilter)}={branchName}&{nameof(BranchFilter.IsEnabledFilter)}={isEnabled}&{expectedBranchesIdsQuery}";

        //    // Act
        //    _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
        //    var response = await _baseTest.HttpClient.GetAsync(uri);
        //    response.EnsureSuccessStatusCode();
        //    var responseString = await response.Content.ReadAsStringAsync();

        //    var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
        //    //Assert
        //    response.StatusCode.Should().Be(StatusCodes.Status200OK);
        //    result.Total.Should().Be(expectedBranches.Count());
        //    result.Data.Count.Should().Be(expectedBranches.Count());
        //    result.Data.All(x => expectedBranches.Select(u => u.Id).Contains(x.Id)).Should().BeTrue();
        //}

        [Theory]
        [InlineData(0, 50)]
        [InlineData(30, 35)]
        public async Task ShouldReturnSuccessResultWithPaginationWhenGetBranchesByCompany(int skip, int take)
        {
            // Arrange
            int branchCount = 100;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();

            var uri = $"company/{company.Id}/{_baseBrqanchUrl}?{nameof(Paginable.Skip)}={skip}&{nameof(Paginable.Take)}={take}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            var branchesWithExpectedCompanyInDb = _baseTest.DbContext.Branches.Where(x => x.CompanyId == company.Id);
            result.Total.Should().Be(branchesWithExpectedCompanyInDb.Count());
            result.Data.Count.Should().Be(branchesWithExpectedCompanyInDb.Skip(skip).Take(take).Count());
            result.ItemsPerPage.Should().Be(take);
        }

        [Theory]
        //------ Default sorting
        //[InlineData(nameof(BranchModel.CreatedDate), "", "")]
        //-----------------------

        [InlineData(nameof(BranchModel.CreatedDate), "createddate", "desc")]
        [InlineData(nameof(BranchModel.CreatedDate), "createddate", "asc")]

        [InlineData(nameof(BranchModel.IsEnabled), "status", "desc")]
        [InlineData(nameof(BranchModel.IsEnabled), "status", "asc")]

        [InlineData(nameof(BranchModel.State), "state", "desc")]
        [InlineData(nameof(BranchModel.State), "state", "asc")]

        [InlineData(nameof(BranchModel.City), "city", "desc")]
        [InlineData(nameof(BranchModel.City), "city", "asc")]

        [InlineData(nameof(BranchModel.BranchName), "branchname", "desc")]
        [InlineData(nameof(BranchModel.BranchName), "branchname", "asc")]

        [InlineData(nameof(BranchModel.Id), "id", "desc")]
        [InlineData(nameof(BranchModel.Id), "id", "asc")]

        /// TODO is valid case
        [InlineData(nameof(BranchModel.IsEnabled), "isenabled", "desc")]
        [InlineData(nameof(BranchModel.IsEnabled), "isenabled", "asc")]
        public async Task ShouldReturnSuccessResultWithSortingWhenGetBranchesByCompany(string sortBy, string sortByRequest, string order)
        {
            // Arrange
            int branchCount = 100;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branches = _baseTest.Fixture.Build<Branch>().With(x => x.Company, company).CreateMany(branchCount).ToList();
            _baseTest.DbContext.Branches.AddRange(branches);
            _baseTest.DbContext.SaveChanges();

            var uri = $"company/{company.Id}/{_baseBrqanchUrl}?{nameof(Sortable.SortBy)}={sortByRequest}&{nameof(Sortable.SortOrder)}={order}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);

            //Assert
            var expression = typeof(BranchModel).GetExpression<BranchModel>(sortBy);

            result.Total.Should().Be(branchCount);
            result.Data.Count.Should().Be(branchCount);
            result.Data.All(x => x.GetType().GetProperty(sortBy).GetValue(x) == null).Should().BeFalse();
            result.ItemsPerPage.Should().Be(0);
            if (order == "asc")
            {
                bool resultSequence = result.Data.SequenceEqual(result.Data.OrderBy(expression));
                resultSequence.Should().BeTrue();
            }
            else
            {
                bool resultSequence = result.Data.SequenceEqual(result.Data.OrderByDescending(expression));
                resultSequence.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWithCorrectDataWhenGetBranchesByCompany()
        {
            // Arrange
            int usersCount = 10;
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            var branchId = Guid.NewGuid();
            var branch = _baseTest.Fixture.Build<Branch>()
                                            .With(x => x.Id, branchId)
                                            .With(x => x.Company, company)
                                            .Create();
            var users = _baseTest.Fixture.Build<Users>()
                                         .With(x => x.Role, _baseTest.SalesRoleId.ToString())
                                         .With(x => x.BranchId, branch.Id)
                                         .CreateMany(usersCount);
            _baseTest.DbContext.Branches.Add(branch);
            _baseTest.DbContext.Users.AddRange(users);
            _baseTest.DbContext.SaveChanges();

            var uri = $"company/{company.Id}/{_baseBrqanchUrl}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<BranchModel>>(responseString);

            //Assert
            result.Total.Should().Be(1);
            result.Data.Count.Should().Be(1);
            result.ItemsPerPage.Should().Be(0);

            var branchModel = result.Data.First();
            branchModel.BranchName.Should().Be(branch.BranchName);
            branchModel.CountSalesRep.Should().Be(usersCount);
            branchModel.Email.Should().Be(branch.Email);
            branchModel.Phone.Should().Be(branch.Phone);
            branchModel.Fax.Should().Be(branch.Fax);
            branchModel.AddressLine1.Should().Be(branch.AddressLine1);
            branchModel.AddressLine2.Should().Be(branch.AddressLine2);
            branchModel.City.Should().Be(branch.City);
            branchModel.ZipCode.Should().Be(branch.ZipCode);
            branchModel.GeoLat.Should().Be(branch.GeoLat);
            branchModel.GeoLng.Should().Be(branch.GeoLng);
            branchModel.State.Should().Be(branch.State);
            branchModel.IsEnabled.Should().Be(branch.IsEnabled);
            branchModel.CreatedDate.Should().Be(branch.CreatedDate);
            branchModel.ModifiedDate.Should().Be(branch.ModifiedDate);
            branchModel.CreatedBy.Should().Be(branch.CreatedBy);
            branchModel.ModifiedBy.Should().Be(branch.ModifiedBy);
            branchModel.CompanyId.Should().Be(branch.CompanyId);
        }

        #endregion

        #region Post Branch

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenPostBranch()
        {
            // Arrange
            string uri = $"{_baseBrqanchUrl}";
            var content = new StringContent(JsonConvert.SerializeObject(string.Empty), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.PostAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfBranchNameIsEmptyWhenPostBranch()
        {
            // Arrange
            string uri = $"{_baseBrqanchUrl}";
            var request = _baseTest.Fixture.Build<CreateBranchModel>()
                                           .With(x => x.BranchName, string.Empty)
                                           .With(x => x.Phone, "066-432-43-56")
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.ZipCode, "7582")
                                           .Without(x => x.BranchContacts)
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PostAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfEmailNoCorrectWhenPostBranch()
        {
            // Arrange
            string uri = $"{_baseBrqanchUrl}";
            var request = _baseTest.Fixture.Build<CreateBranchModel>()
                                           .With(x => x.Phone, "066-432-43-56")
                                           .With(x => x.ZipCode, "7582")
                                           .Without(x => x.BranchContacts)
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PostAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfPhoneNoCorrectWhenPostBranch()
        {
            // Arrange
            string uri = $"{_baseBrqanchUrl}";
            var request = _baseTest.Fixture.Build<CreateBranchModel>()
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.ZipCode, "7582")
                                           .Without(x => x.BranchContacts)
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PostAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfZipCodeNoCorrectWhenPostBranch()
        {
            // Arrange
            string uri = $"{_baseBrqanchUrl}";
            var request = _baseTest.Fixture.Build<CreateBranchModel>()
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.Phone, "066-432-43-56")
                                           .With(x=>x.ZipCode, "testZipCode")
                                           .Without(x => x.BranchContacts)
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PostAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWhenPostBranch()
        {
            // Arrange
            string uri = $"{_baseBrqanchUrl}";
            var request = _baseTest.Fixture.Build<CreateBranchModel>()
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.Phone, "066-432-43-56")
                                           .With(x => x.ZipCode, "578")
                                           .Without(x=>x.BranchContacts)
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Guid>(responseString);
            var branchFromDb = _baseTest.DbContext.Branches.First(x => x.BranchName == request.BranchName);
            //Assert
            result.Should().Be(branchFromDb.Id);
        }
        #endregion

        #region Put Branch

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenPutBranch()
        {
            // Arrange
            Guid branchId = Guid.NewGuid();

            string uri = $"{_baseBrqanchUrl}/{branchId}";
            var content = new StringContent(JsonConvert.SerializeObject(string.Empty), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.PutAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfBranchNameIsEmptyWhenPutBranch()
        {
            // Arrange
            Guid branchId = Guid.NewGuid();

            string uri = $"{_baseBrqanchUrl}/{branchId}";
            var request = _baseTest.Fixture.Build<CreateBranchModel>()
                                            .With(x => x.BranchName, string.Empty)
                                            .With(x => x.Phone, "066-432-43-56")
                                            .With(x => x.Email, "test@email.com")
                                            .With(x => x.ZipCode, "7582")
                                            .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfEmailNoCorrectWhenPutBranch()
        {
            // Arrange
            Guid branchId = Guid.NewGuid();

            string uri = $"{_baseBrqanchUrl}/{branchId}";
            var request = _baseTest.Fixture.Build<CreateBranchModel>()
                                            .With(x => x.Phone, "066-432-43-56")
                                            .With(x => x.ZipCode, "7582")
                                            .Without(x=>x.BranchContacts)
                                            .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfPhoneWhenPutBranch()
        {
            // Arrange
            Guid branchId = Guid.NewGuid();

            string uri = $"{_baseBrqanchUrl}/{branchId}";
            var request = _baseTest.Fixture.Build<CreateBranchModel>()
                                            .With(x => x.Email, "test@email.com")
                                            .With(x => x.ZipCode, "7582")
                                            .Without(x => x.BranchContacts)
                                            .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfZipCodeNoCorrectWhenPutBranch()
        {
            // Arrange
            Guid branchId = Guid.NewGuid();

            string uri = $"{_baseBrqanchUrl}/{branchId}";
            var request = _baseTest.Fixture.Build<CreateBranchModel>()
                                            .With(x => x.Email, "test@email.com")
                                            .With(x => x.Phone, "066-432-43-56")
                                            .With(x=>x.ZipCode, "testZipCode")
                                            .Without(x => x.BranchContacts)
                                            .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultIfBranchNotExistWhenPutBranch()
        {
            // Arrange
            Guid branchId = Guid.NewGuid();

            string uri = $"{_baseBrqanchUrl}/{branchId}";
            var request = _baseTest.Fixture.Build<CreateBranchModel>()
                                            .With(x => x.Email, "test@email.com")
                                            .With(x => x.Phone, "066-432-43-56")
                                            .With(x => x.ZipCode, "7582")
                                            .Without(x => x.BranchContacts)
                                            .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWhenPutBranch()
        {
            // Arrange
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>().With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id).Create();
            _baseTest.DbContext.Companies.Add(company);
            _baseTest.DbContext.SaveChanges();
            var branch = _baseTest.Fixture.Build<Branch>()
                                          .With(x => x.Id, Guid.NewGuid())
                                          .With(x=>x.CompanyId, company.Id)
                                          .Create();
            _baseTest.DbContext.Branches.Add(branch);
            _baseTest.DbContext.SaveChanges();

            string uri = $"{_baseBrqanchUrl}/{branch.Id}";
            var request = _baseTest.Fixture.Build<CreateBranchModel>()
                                            .With(x => x.Email, "test@email.com")
                                            .With(x => x.Phone, "066-432-43-56")
                                            .With(x => x.ZipCode, "7582")
                                            .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, content);
            response.EnsureSuccessStatusCode();

            await _baseTest.DbContext.Entry(branch).ReloadAsync();
            var branchFromDb = _baseTest.DbContext.Branches.First(x => x.Id == branch.Id);
            //Assert
            branch.Id.Should().Be(branch.Id);
            branch.BranchName.Should().Be(request.BranchName);
            branch.Email.Should().Be(request.Email);
            branch.Phone.Should().Be(request.Phone);
            branch.Fax.Should().Be(request.Fax);
            branch.State.Should().Be(request.State);
            branch.City.Should().Be(request.City);
            branch.AddressLine1.Should().Be(request.AddressLine1);
            branch.AddressLine2.Should().Be(request.AddressLine2);
            branch.ZipCode.Should().Be(request.ZipCode);
            branch.GeoLat.Should().Be(request.GeoLat);
            branch.GeoLng.Should().Be(request.GeoLng);
            //branch.IsStatusActive.Should().Be(request.IsStatusActive);
            branch.CompanyId.Should().Be(request.CompanyId);
            branch.IsEnabled.Should().Be(request.IsEnabled);
        }

        #endregion

        #region Patch Branch

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenPatchBranch()
        {
            // Arrange
            Guid branchId = Guid.NewGuid();

            string uri = $"{_baseBrqanchUrl}/{branchId}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.PatchAsync(uri, null);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultIfBranchNotExistWhenPatchBranch()
        {
            // Arrange
            Guid branchId = Guid.NewGuid();

            string uri = $"{_baseBrqanchUrl}/{branchId}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PatchAsync(uri, null);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultIfBranchExistButRequestStatusNotOnBoardedWhenPatchBranch()
        {
            // Arrange
            var branch = _baseTest.Fixture.Build<Branch>()
                                            .With(x => x.Id, Guid.NewGuid())
                                            .Create();
            _baseTest.DbContext.Branches.Add(branch);
            _baseTest.DbContext.SaveChanges();
            string uri = $"{_baseBrqanchUrl}/{branch.Id}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PatchAsync(uri, null);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWhenPatchBranch()
        {
            // Arrange
            var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .Create();
            var branch = _baseTest.Fixture.Build<Branch>()
                                            .With(x => x.Company, company)
                                            .With(x=>x.IsEnabled, false)
                                            .Create();
            _baseTest.DbContext.Branches.Add(branch);
            _baseTest.DbContext.SaveChanges();
            string uri = $"{_baseBrqanchUrl}/{branch.Id}?isEnabled={true}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PatchAsync(uri, null);
            response.EnsureSuccessStatusCode();

            await _baseTest.DbContext.Entry(branch).ReloadAsync();
            var branchFromDb = _baseTest.DbContext.Branches.First(x => x.Id == branch.Id);
            //Assert
            branchFromDb.IsEnabled.Should().BeTrue();
        }

        #endregion

        #region GetAnyBranchAsync

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWheGetAnyBranchAsync()
        {
            // Arrange
            string trustedToken = _baseTest.Fixture.Create<string>();
            string uri = $"{_baseBrqanchUrl}/{trustedToken}/trusted/internal";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnForbiddenResultWheGetAnyBranchAsync()
        {
            // Arrange
            string trustedToken = _baseTest.Fixture.Create<string>();
            string uri = $"{_baseBrqanchUrl}/{trustedToken}/trusted/internal";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task ShouldReturnNoFoundByIdResultIfBranchNotExistWheGetAnyBranchAsync()
        {
            // Arrange
            var branch = _baseTest.Fixture.Create<Branch>();
            _baseTest.DbContext.Branches.Add(branch);
            _baseTest.DbContext.SaveChanges();

            string uri = $"{_baseBrqanchUrl}/{Consts.StaticToken}/trusted/internal?{nameof(BranchMinRequestModel.Id)}={Guid.NewGuid()}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }


        [Fact]
        public async Task ShouldReturnSuccessByIdResultWheGetAnyBranchAsync()
        {
            // Arrange
            var branch = _baseTest.Fixture.Create<Branch>();
            _baseTest.DbContext.Branches.Add(branch);
            _baseTest.DbContext.SaveChanges();

            string uri = $"{_baseBrqanchUrl}/{Consts.StaticToken}/trusted/internal?{nameof(BranchMinRequestModel.BranchName)}={branch.BranchName}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<BranchMin>(responseString);
            //Assert

            result.Id.Should().Be(branch.Id);
            result.BranchName.Should().Be(branch.BranchName);
            result.CreatedDate.Should().Be(branch.CreatedDate);
        }

        [Fact]
        public async Task ShouldReturnSuccessByNameResultWheGetAnyBranchAsync()
        {
            // Arrange
            var branch = _baseTest.Fixture.Create<Branch>();
            _baseTest.DbContext.Branches.Add(branch);
            _baseTest.DbContext.SaveChanges();

            string uri = $"{_baseBrqanchUrl}/{Consts.StaticToken}/trusted/internal?{nameof(BranchMinRequestModel.Id)}={branch.Id}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<BranchMin>(responseString);
            //Assert

            result.Id.Should().Be(branch.Id);
            result.BranchName.Should().Be(branch.BranchName);
            result.CreatedDate.Should().Be(branch.CreatedDate);
        }

        #endregion
    }
}
