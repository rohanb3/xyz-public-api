using FluentAssertions;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;
using Xyzies.TWC.Public.Api.Models;
using System.Linq;
using Xyzies.TWC.Public.Data.Entities;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Xyzies.TWC.Public.Api.Tests.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;
using Xyzies.TWC.Public.Data.Repositories.Azure;
using Xyzies.TWC.Public.Data.Entities.Azure;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using Moq;
using System.Text;
using System.Net.Http;

namespace Xyzies.TWC.Public.Api.Tests.IntegrationTests.Controllers
{
    public class CompanyControllerTest : IClassFixture<BaseTest>
    {
        private readonly BaseTest _baseTest = null;
        private readonly string _baseCompanyUrl = null;

        public CompanyControllerTest(BaseTest baseTest)
        {
            _baseTest = baseTest ?? throw new ArgumentNullException(nameof(baseTest));
            _baseTest.CableDbContext.ClearContext();
            _baseTest.TestSeed.Seed();
            _baseTest.CableDbContext.RemoveRange(_baseTest.CableDbContext.Companies);
            _baseTest.CableDbContext.SaveChanges();
            _baseCompanyUrl = "company";
        }

        #region Get branches list

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenGetCompanies()
        {
            // Arrange

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.GetAsync(_baseCompanyUrl);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByStateWhenGetCompanies()
        {
            // Arrange
            int countCompanyWithOneState = 3;
            int countCompanyWithDifferenceState = 5;
            string state = _baseTest.Fixture.Create<string>();
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompanyWithDifferenceState)
                                           .ToList();
            companies.AddRange(_baseTest.Fixture.Build<Company>()
                                                .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                                .With(x => x.State, state)
                                                .CreateMany(countCompanyWithOneState));
            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}?{nameof(CompanyFilter.StateFilter)}={state}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(countCompanyWithOneState);
            result.Data.Count.Should().Be(countCompanyWithOneState);
            result.Data.All(x => x.State.ToLower() == state.ToLower()).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultIfCompanyHasNotRequestStatusWhenGetCompanies()
        {
            // Arrange
            var company = _baseTest.Fixture.Create<Company>();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.FirstOrDefault(x => x.Id == company.Id).Should().BeNull();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByCityWhenGetCompanies()
        {
            // Arrange
            int countCompanyWithOneCity = 3;
            int countCompanyWithDifferenceCity = 5;
            string city = _baseTest.Fixture.Create<string>();
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompanyWithDifferenceCity)
                                           .ToList();
            companies.AddRange(_baseTest.Fixture.Build<Company>()
                                                .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                                .With(x => x.City, city)
                                                .CreateMany(countCompanyWithOneCity));
            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}?{nameof(CompanyFilter.CityFilter)}={city}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(countCompanyWithOneCity);
            result.Data.Count.Should().Be(countCompanyWithOneCity);
            result.Data.All(x => x.City.ToLower().Contains(city.ToLower())).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByEmailWhenGetCompanies()
        {
            // Arrange
            int countCompanyWithOneEmail = 3;
            int countCompanyWithDifferenceEmail = 5;
            string email = _baseTest.Fixture.Create<string>();
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompanyWithDifferenceEmail)
                                           .ToList();
            companies.AddRange(_baseTest.Fixture.Build<Company>()
                                                .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                                .With(x => x.Email, email)
                                                .CreateMany(countCompanyWithOneEmail));
            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}?{nameof(CompanyFilter.EmailFilter)}={email}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(countCompanyWithOneEmail);
            result.Data.Count.Should().Be(countCompanyWithOneEmail);
            result.Data.All(x => x.Email.ToLower().Contains(email.ToLower())).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByCompanyNamesWhenGetCompanies()
        {
            // Arrange
            int countCompanyMustBeFounded = 3;
            int countCompany = 10;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany)
                                           .ToList();

            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            var expectedCompanies = _baseTest.CableDbContext.Companies.Take(countCompanyMustBeFounded);
            var expectedCompanyNameQuery = string.Join('&', expectedCompanies.Select(x => $"{nameof(CompanyFilter.CompanyNameFilter)}={x.CompanyName}"));
            string uri = $"{_baseCompanyUrl}?{expectedCompanyNameQuery}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(countCompanyMustBeFounded);
            result.Data.Count.Should().Be(countCompanyMustBeFounded);
            result.Data.All(x => expectedCompanies.Select(c => c.CompanyName).Contains(x.CompanyName)).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByDateFromWhenGetCompanies()
        {
            // Arrange
            var dateCreated = DateTime.Now;
            int countCompanyMustBeFounded = 3;
            int countCompany = 10;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .With(x => x.CreatedDate, DateTime.Now.AddDays(-1))
                                           .CreateMany(countCompany)
                                           .ToList();
            var expectedCompanies = companies.Take(countCompanyMustBeFounded).ToList();
            for (int i = 0; i < expectedCompanies.Count; i++)
            {
                expectedCompanies[i].CreatedDate = dateCreated.AddDays(i);
            }
            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}?{nameof(CompanyFilter.DateFrom)}={dateCreated.ToString("yyyy-MM-dd")}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(countCompanyMustBeFounded);
            result.Data.Count.Should().Be(countCompanyMustBeFounded);
            result.Data.All(x => x.CreatedDate >= dateCreated).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByDateToWhenGetCompanies()
        {
            // Arrange
            var dateCreated = DateTime.Now;
            int countCompanyMustBeFounded = 3;
            int countCompany = 10;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .With(x => x.CreatedDate, DateTime.Now.AddDays(+1))
                                           .CreateMany(countCompany)
                                           .ToList();
            var expectedCompanies = companies.Take(countCompanyMustBeFounded).ToList();
            for (int i = 0; i < expectedCompanies.Count; i++)
            {
                expectedCompanies[i].CreatedDate = dateCreated.AddDays(-(i + 1));
            }
            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}?{nameof(CompanyFilter.DateTo)}={dateCreated.ToString("yyyy-MM-dd")}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(countCompanyMustBeFounded);
            result.Data.Count.Should().Be(countCompanyMustBeFounded);
            result.Data.All(x => x.CreatedDate <= dateCreated).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByDateFromAndDateToWhenGetCompanies()
        {
            // Arrange
            int countCompany = 10;
            int countCompanyMustBeFounded = 4;
            var dateCreatedFrom = DateTime.Now;
            var dateCreatedTo = DateTime.Now.AddDays(countCompanyMustBeFounded);
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany)
                                           .ToList();
            for (int i = 0; i < companies.Count; i++)
            {
                companies[i].CreatedDate = dateCreatedFrom.AddDays(i);
            }
            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}?{nameof(CompanyFilter.DateFrom)}={dateCreatedFrom.ToString("yyyy-MM-dd")}&{nameof(CompanyFilter.DateTo)}={dateCreatedTo.ToString("yyyy-MM-dd")}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(countCompanyMustBeFounded);
            result.Data.Count.Should().Be(countCompanyMustBeFounded);
            result.Data.All(x => x.CreatedDate >= dateCreatedFrom && x.CreatedDate <= dateCreatedTo).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByEnabledWhenGetCompanies()
        {
            // Arrange
            int countCompany = 10;
            bool isEnabled = true;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany)
                                           .ToList();
            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}?{nameof(CompanyFilter.IsEnabledFilter)}={isEnabled}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            int countIsEnabledEntity = _baseTest.CableDbContext.Companies.Count(x => x.IsEnabled == isEnabled);
            result.Total.Should().Be(countIsEnabledEntity);
            result.Data.Count.Should().Be(countIsEnabledEntity);
            result.Data.All(x => x.IsEnabled == isEnabled).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByListIdWhenGetCompanies()
        {
            // Arrange
            int countCompany = 10;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany)
                                           .ToList();
            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            var expectedCompany = companies.First();

            string uri = $"{_baseCompanyUrl}?{nameof(CompanyFilter.CompanyIdFilter)}={expectedCompany.Id}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(1);
            result.Data.Count.Should().Be(1);
            result.Data.First().Id.Should().Be(expectedCompany.Id);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByIdWhenGetCompanies()
        {
            // Arrange
            int countCompany = 10;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany)
                                           .ToList();
            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            var expectedCompany = companies.First();

            string uri = $"{_baseCompanyUrl}?{nameof(CompanyFilter.CompanyIdFilter)}={expectedCompany.Id}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(1);
            result.Data.Count.Should().Be(1);
            result.Data.First().Id.Should().Be(expectedCompany.Id);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByCompanyNameWhenGetCompanies()
        {
            // Arrange
            int countCompany = 10;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany)
                                           .ToList();
            string companyNameStartWith = "Test";
            companies.ForEach(x => x.CompanyName = $"{companyNameStartWith}{x.CompanyName}");
            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            string expectedContainsCompanyName = $"{companyNameStartWith}{nameof(Company.CompanyName)}";
            string uri = $"{_baseCompanyUrl}?{nameof(CompanyFilter.SearchFilter)}={expectedContainsCompanyName}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(countCompany);
            result.Data.Count.Should().Be(countCompany);
            result.Data.All(x => x.CompanyName.ToLower().Contains(expectedContainsCompanyName));
        }

        [Fact]
        public async Task ShouldReturnSuccessResultFileredByCompanyIdsWithoutOthersFilterWhenGetCompanies()
        {
            // Arrange
            int countCompanyMustBeFounded = 3;
            int countCompany = 10;
            string state = _baseTest.Fixture.Create<string>();
            string city = _baseTest.Fixture.Create<string>();
            string email = _baseTest.Fixture.Create<string>();
            string companyName = _baseTest.Fixture.Create<string>();
            var dateFrom = DateTime.Now.AddDays(-1);
            var dateTo = DateTime.Now.AddDays(1);
            var dateCreated = DateTime.Now;
            bool isEnabled = true;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .With(x => x.State, state)
                                           .With(x => x.City, city)
                                           .With(x => x.Email, email)
                                           .With(x => x.CompanyName, companyName)
                                           .With(x => x.IsEnabled, isEnabled)
                                           .With(x => x.CreatedDate, dateCreated)
                                           .CreateMany(countCompany)
                                           .ToList();

            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            var expectedCompanies = _baseTest.CableDbContext.Companies.Take(countCompanyMustBeFounded);
            var expectedCompanyIdQuery = string.Join('&', expectedCompanies.Select(x => $"{nameof(CompanyFilter.CompanyIds)}={x.Id}"));
            string uri = $"{_baseCompanyUrl}?{expectedCompanyIdQuery}&{nameof(CompanyFilter.StateFilter)}={state}&{nameof(CompanyFilter.CityFilter)}={city}&{nameof(CompanyFilter.EmailFilter)}={email}&{nameof(CompanyFilter.IsEnabledFilter)}={isEnabled}&{nameof(CompanyFilter.SearchFilter)}={companyName}&{nameof(CompanyFilter.DateFrom)}={dateFrom.ToString("yyyy-MM-dd")}&{nameof(CompanyFilter.DateTo)}={dateTo.ToString("yyyy-MM-dd")}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyMin>>(responseString);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(countCompanyMustBeFounded);
            result.Data.Count.Should().Be(countCompanyMustBeFounded);
            result.Data.All(x => expectedCompanies.Select(c => c.Id).Contains(x.Id)).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultCheckCorrectDataWhenGetCompanies()
        {
            // Arrange
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            int branchCount = 5;
            int usersCount = 5;
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .Create();
            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();

            var branches = _baseTest.Fixture.Build<Branch>()
                                            .With(x => x.CompanyId, company.Id)
                                            .CreateMany(branchCount);

            var users = _baseTest.Fixture.Build<Users>()
                                       .With(x => x.Role, _baseTest.SalesRoleId.ToString())
                                       .With(x => x.CompanyId, company.Id)
                                       .CreateMany(usersCount);

            _baseTest.CableDbContext.Branches.AddRange(branches);
            _baseTest.CableDbContext.Users.AddRange(users);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            int countCompanyInDb = _baseTest.CableDbContext.Companies.Count();
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(countCompanyInDb);
            result.Data.Count.Should().Be(countCompanyInDb);
            var companyResult = result.Data.First(x => x.Id == company.Id);
            companyResult.CountBranch.Should().Be(branchCount);
            companyResult.CountSalesRep.Should().Be(usersCount);
            companyResult.CompanyName.Should().Be(company.CompanyName);
            companyResult.LegalName.Should().Be(company.LegalName);
            companyResult.Email.Should().Be(company.Email);
            companyResult.Phone.Should().Be(company.Phone);
            companyResult.Address.Should().Be(company.Address);
            companyResult.City.Should().Be(company.City);
            companyResult.State.Should().Be(company.State);
            companyResult.ZipCode.Should().Be(company.ZipCode);
            companyResult.StoreID.Should().Be(company.StoreID);
            companyResult.CreatedDate.Should().Be(company.CreatedDate);
            companyResult.ModifiedDate.Should().Be(company.ModifiedDate);
            companyResult.CreatedBy.Should().Be(company.CreatedBy);
            companyResult.ModifiedBy.Should().Be(company.ModifiedBy);
            companyResult.Agentid.Should().Be(company.Agentid);
            companyResult.Status.Should().Be(company.Status);
            companyResult.StoreLocationCount.Should().Be(company.StoreLocationCount);
            companyResult.PrimaryContactName.Should().Be(company.PrimaryContactName);
            companyResult.PrimaryContactTitle.Should().Be(company.PrimaryContactTitle);
            companyResult.IsEnabled.Should().Be(company.IsEnabled);
            companyResult.Fax.Should().Be(company.Fax);
            companyResult.FedId.Should().Be(company.FedId);
            companyResult.TypeOfCompany.Should().Be(company.TypeOfCompany);
            companyResult.StateEstablished.Should().Be(company.StateEstablished);
            companyResult.CompanyType.Should().Be(company.CompanyType);
            companyResult.CallerId.Should().Be(company.CallerId);
            companyResult.IsAgreement.Should().Be(company.IsAgreement.Value);
            companyResult.ActivityStatus.Should().Be(company.ActivityStatus);
            companyResult.CompanyKey.Should().Be(company.CompanyKey?.ToString());
            companyResult.FirstName.Should().Be(company.FirstName);
            companyResult.LastName.Should().Be(company.LastName);
            companyResult.CellNumber.Should().Be(company.CellNumber);
            companyResult.BankNumber.Should().Be(company.BankNumber);
            companyResult.BankName.Should().Be(company.BankName);
            companyResult.BankAccountNumber.Should().Be(company.BankAccountNumber);
            companyResult.XyziesId.Should().Be(company.XyziesId);
            companyResult.ApprovedDate.Should().Be(company.ApprovedDate.Value);
            companyResult.BankInfoGiven.Should().Be(company.BankInfoGiven.Value);
            companyResult.AccountManager.Should().Be(company.AccountManager.Value);
            companyResult.CrmCompanyId.Should().Be(company.CrmCompanyId);
            companyResult.IsCallCenter.Should().Be(company.IsCallCenter.Value);
            companyResult.ParentCompanyId.Should().Be(company.ParentCompanyId);
            companyResult.TeamKey.Should().Be(company.TeamKey.Value);
            companyResult.RetailerGroupKey.Should().Be(company.RetailerGroupKey.Value);
            companyResult.SocialMediaAccount.Should().Be(company.SocialMediaAccount);
            companyResult.RetailerGoogleAccount.Should().Be(company.RetailerGoogleAccount);
            companyResult.RetailerGooglePassword.Should().Be(company.RetailerGooglePassword);
            companyResult.PaymentMode.Should().Be(company.PaymentMode);
            companyResult.CustomerDemographicId.Should().Be(company.CustomerDemographicId);
            companyResult.LocationTypeId.Should().Be(company.LocationTypeId);
            companyResult.IsOwnerPassBackground.Should().Be(company.IsOwnerPassBackground.Value);
            companyResult.IsWebsite.Should().Be(company.IsWebsite.Value);
            companyResult.IsSellsLifelineWireless.Should().Be(company.IsSellsLifelineWireless.Value);
            companyResult.NumberofStores.Should().Be(company.NumberofStores);
            companyResult.BusinessDescription.Should().Be(company.BusinessDescription);
            companyResult.WebsiteList.Should().Be(company.WebsiteList);
            companyResult.IsSpectrum.Should().Be(company.IsSpectrum.Value);
            companyResult.BusinessSource.Should().Be(company.BusinessSource);
            companyResult.GeoLat.Should().Be(company.GeoLat);
            companyResult.GeoLon.Should().Be(company.GeoLon);
            companyResult.IsMarketPlace.Should().Be(company.IsMarketPlace.Value);
            companyResult.MarketPlaceName.Should().Be(company.MarketPlaceName);
            companyResult.PhysicalName.Should().Be(company.PhysicalName);
            companyResult.MarketStrategy.Should().Be(company.MarketStrategy);
            companyResult.CompanyStatusKey.Should().Be(company.CompanyStatusKey);
        }

        [Theory]
        [InlineData(0, 50)]
        [InlineData(30, 35)]
        public async Task ShouldReturnSuccessResulttWithPaginationWhenGetCompanies(int skip, int take)
        {
            // Arrange
            int countCompany = 100;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany);

            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}?{nameof(Paginable.Skip)}={skip}&{nameof(Paginable.Take)}={take}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            int countCompanyInDb = _baseTest.CableDbContext.Companies.Count();

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Total.Should().Be(_baseTest.CableDbContext.Companies.Count());
            result.Data.Count.Should().Be(_baseTest.CableDbContext.Companies.Skip(skip).Take(take).Count());
            result.ItemsPerPage.Should().Be(take);
        }

        [Theory]
        //------ Default sorting
        //[InlineData(nameof(CompanyModel.CreatedDate), "", "")]
        //-----------------------

        [InlineData(nameof(CompanyModel.CreatedDate), "createddate", "desc")]
        [InlineData(nameof(CompanyModel.CreatedDate), "createddate", "asc")]

        [InlineData(nameof(CompanyModel.Status), "status", "desc")]
        [InlineData(nameof(CompanyModel.Status), "status", "asc")]

        [InlineData(nameof(CompanyModel.State), "state", "desc")]
        [InlineData(nameof(CompanyModel.State), "state", "asc")]

        [InlineData(nameof(CompanyModel.City), "city", "desc")]
        [InlineData(nameof(CompanyModel.City), "city", "asc")]

        [InlineData(nameof(CompanyModel.CompanyName), "companyname", "desc")]
        [InlineData(nameof(CompanyModel.CompanyName), "companyname", "asc")]

        [InlineData(nameof(CompanyModel.Id), "id", "desc")]
        [InlineData(nameof(CompanyModel.Id), "id", "asc")]
        public async Task ShouldReturnSuccessResulttWithSortingWhenGetCompanies(string sortBy, string sortByRequest, string order)
        {
            // Arrange
            int countCompany = 100;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany);

            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}?{nameof(Sortable.SortBy)}={sortByRequest}&{nameof(Sortable.SortOrder)}={order}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            int countCompanyInDb = _baseTest.CableDbContext.Companies.Count();

            //Assert
            var expression = typeof(CompanyModel).GetExpression<CompanyModel>(sortBy);

            result.Total.Should().Be(countCompanyInDb);
            result.Data.Count.Should().Be(countCompanyInDb);
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
        #endregion

        #region Get company list by trusted token

        [Fact]
        public async Task ShouldReturnForbiddenResultWhenGetCompaniesByTrustedToken()
        {
            // Arrange
            string token = _baseTest.Fixture.Create<string>();
            string uri = $"{_baseCompanyUrl}/{token}/trusted";
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWhenGetCompaniesByTrustedToken()
        {
            // Arrange
            int countCompany = 100;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany);

            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();
            string uri = $"{_baseCompanyUrl}/{Consts.StaticToken}/trusted";
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            int countCompanyInDb = _baseTest.CableDbContext.Companies.Count();
            //Assert
            result.Total.Should().Be(countCompanyInDb);
            result.Data.Count.Should().Be(countCompanyInDb);
            result.ItemsPerPage.Should().Be(0);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWithCheckCorrectDataWhenGetCompaniesByTrustedToken()
        {
            // Arrange
            int usersCount = 5;
            int branchCount = 9;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .Create();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();

            var branches = _baseTest.Fixture.Build<Branch>()
                                           .With(x => x.CompanyId, company.Id)
                                           .CreateMany(branchCount);

            var users = _baseTest.Fixture.Build<Users>()
                                       .With(x => x.Role, _baseTest.SalesRoleId.ToString())
                                       .With(x => x.CompanyId, company.Id)
                                       .CreateMany(usersCount);

            _baseTest.CableDbContext.Branches.AddRange(branches);
            _baseTest.CableDbContext.Users.AddRange(users);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}/{Consts.StaticToken}/trusted";
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagingResult<CompanyModel>>(responseString);
            int countCompanyInDb = _baseTest.CableDbContext.Companies.Count();
            //Assert
            result.Total.Should().Be(countCompanyInDb);
            result.Data.Count.Should().Be(countCompanyInDb);
            result.ItemsPerPage.Should().Be(0);
            var companyResult = result.Data.First(x => x.Id == company.Id);
            companyResult.CountBranch.Should().Be(branchCount);
            companyResult.CountSalesRep.Should().Be(usersCount);
            companyResult.CompanyName.Should().Be(company.CompanyName);
            companyResult.LegalName.Should().Be(company.LegalName);
            companyResult.Email.Should().Be(company.Email);
            companyResult.Phone.Should().Be(company.Phone);
            companyResult.Address.Should().Be(company.Address);
            companyResult.City.Should().Be(company.City);
            companyResult.State.Should().Be(company.State);
            companyResult.ZipCode.Should().Be(company.ZipCode);
            companyResult.StoreID.Should().Be(company.StoreID);
            companyResult.CreatedDate.Should().Be(company.CreatedDate);
            companyResult.ModifiedDate.Should().Be(company.ModifiedDate);
            companyResult.CreatedBy.Should().Be(company.CreatedBy);
            companyResult.ModifiedBy.Should().Be(company.ModifiedBy);
            companyResult.Agentid.Should().Be(company.Agentid);
            companyResult.Status.Should().Be(company.Status);
            companyResult.StoreLocationCount.Should().Be(company.StoreLocationCount);
            companyResult.PrimaryContactName.Should().Be(company.PrimaryContactName);
            companyResult.PrimaryContactTitle.Should().Be(company.PrimaryContactTitle);
            companyResult.IsEnabled.Should().Be(company.IsEnabled);
            companyResult.Fax.Should().Be(company.Fax);
            companyResult.FedId.Should().Be(company.FedId);
            companyResult.TypeOfCompany.Should().Be(company.TypeOfCompany);
            companyResult.StateEstablished.Should().Be(company.StateEstablished);
            companyResult.CompanyType.Should().Be(company.CompanyType);
            companyResult.CallerId.Should().Be(company.CallerId);
            companyResult.IsAgreement.Should().Be(company.IsAgreement.Value);
            companyResult.ActivityStatus.Should().Be(company.ActivityStatus);
            companyResult.CompanyKey.Should().Be(company.CompanyKey?.ToString());
            companyResult.FirstName.Should().Be(company.FirstName);
            companyResult.LastName.Should().Be(company.LastName);
            companyResult.CellNumber.Should().Be(company.CellNumber);
            companyResult.BankNumber.Should().Be(company.BankNumber);
            companyResult.BankName.Should().Be(company.BankName);
            companyResult.BankAccountNumber.Should().Be(company.BankAccountNumber);
            companyResult.XyziesId.Should().Be(company.XyziesId);
            companyResult.ApprovedDate.Should().Be(company.ApprovedDate.Value);
            companyResult.BankInfoGiven.Should().Be(company.BankInfoGiven.Value);
            companyResult.AccountManager.Should().Be(company.AccountManager.Value);
            companyResult.CrmCompanyId.Should().Be(company.CrmCompanyId);
            companyResult.IsCallCenter.Should().Be(company.IsCallCenter.Value);
            companyResult.ParentCompanyId.Should().Be(company.ParentCompanyId);
            companyResult.TeamKey.Should().Be(company.TeamKey.Value);
            companyResult.RetailerGroupKey.Should().Be(company.RetailerGroupKey.Value);
            companyResult.SocialMediaAccount.Should().Be(company.SocialMediaAccount);
            companyResult.RetailerGoogleAccount.Should().Be(company.RetailerGoogleAccount);
            companyResult.RetailerGooglePassword.Should().Be(company.RetailerGooglePassword);
            companyResult.PaymentMode.Should().Be(company.PaymentMode);
            companyResult.CustomerDemographicId.Should().Be(company.CustomerDemographicId);
            companyResult.LocationTypeId.Should().Be(company.LocationTypeId);
            companyResult.IsOwnerPassBackground.Should().Be(company.IsOwnerPassBackground.Value);
            companyResult.IsWebsite.Should().Be(company.IsWebsite.Value);
            companyResult.IsSellsLifelineWireless.Should().Be(company.IsSellsLifelineWireless.Value);
            companyResult.NumberofStores.Should().Be(company.NumberofStores);
            companyResult.BusinessDescription.Should().Be(company.BusinessDescription);
            companyResult.WebsiteList.Should().Be(company.WebsiteList);
            companyResult.IsSpectrum.Should().Be(company.IsSpectrum.Value);
            companyResult.BusinessSource.Should().Be(company.BusinessSource);
            companyResult.GeoLat.Should().Be(company.GeoLat);
            companyResult.GeoLon.Should().Be(company.GeoLon);
            companyResult.IsMarketPlace.Should().Be(company.IsMarketPlace.Value);
            companyResult.MarketPlaceName.Should().Be(company.MarketPlaceName);
            companyResult.PhysicalName.Should().Be(company.PhysicalName);
            companyResult.MarketStrategy.Should().Be(company.MarketStrategy);
            companyResult.CompanyStatusKey.Should().Be(company.CompanyStatusKey);

        }
        #endregion

        #region Get company by id

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenGetCompanyById()
        {
            // Arrange
            var id = _baseTest.Fixture.Create<int>();
            string uri = $"{_baseCompanyUrl}/{id}";
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }


        [Fact]
        public async Task ShouldReturnNotFoundIfCompanyNotExistResultWhenGetCompanyById()
        {
            // Arrange
            var id = _baseTest.Fixture.Create<int>();
            string uri = $"{_baseCompanyUrl}/{id}";
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWithoutLogoWhenGetCompanyById()
        {
            // Arrange
            int usersCount = 5;
            int branchCount = 9;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .With(x => x.Id, _baseTest.DefaultCompanyId)
                                           .Create();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();

            var branches = _baseTest.Fixture.Build<Branch>()
                                           .With(x => x.CompanyId, company.Id)
                                           .CreateMany(branchCount);

            var users = _baseTest.Fixture.Build<Users>()
                                       .With(x => x.Role, _baseTest.SalesRoleId.ToString())
                                       .With(x => x.CompanyId, company.Id)
                                       .CreateMany(usersCount);

            _baseTest.CableDbContext.Branches.AddRange(branches);
            _baseTest.CableDbContext.Users.AddRange(users);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}/{company.Id}";
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var companyResult = JsonConvert.DeserializeObject<CompanyModelExtended>(responseString);
            //Assert
            companyResult.CountBranch.Should().Be(branchCount);
            companyResult.CountSalesRep.Should().Be(usersCount);
            companyResult.CompanyName.Should().Be(company.CompanyName);
            companyResult.LegalName.Should().Be(company.LegalName);
            companyResult.Email.Should().Be(company.Email);
            companyResult.Phone.Should().Be(company.Phone);
            companyResult.Address.Should().Be(company.Address);
            companyResult.City.Should().Be(company.City);
            companyResult.State.Should().Be(company.State);
            companyResult.ZipCode.Should().Be(company.ZipCode);
            companyResult.StoreID.Should().Be(company.StoreID);
            companyResult.CreatedDate.Should().Be(company.CreatedDate);
            companyResult.ModifiedDate.Should().Be(company.ModifiedDate);
            companyResult.CreatedBy.Should().Be(company.CreatedBy);
            companyResult.ModifiedBy.Should().Be(company.ModifiedBy);
            companyResult.Agentid.Should().Be(company.Agentid);
            companyResult.Status.Should().Be(company.Status);
            companyResult.StoreLocationCount.Should().Be(company.StoreLocationCount);
            companyResult.PrimaryContactName.Should().Be(company.PrimaryContactName);
            companyResult.PrimaryContactTitle.Should().Be(company.PrimaryContactTitle);
            companyResult.IsEnabled.Should().Be(company.IsEnabled);
            companyResult.Fax.Should().Be(company.Fax);
            companyResult.FedId.Should().Be(company.FedId);
            companyResult.TypeOfCompany.Should().Be(company.TypeOfCompany);
            companyResult.StateEstablished.Should().Be(company.StateEstablished);
            companyResult.CompanyType.Should().Be(company.CompanyType);
            companyResult.CallerId.Should().Be(company.CallerId);
            companyResult.IsAgreement.Should().Be(company.IsAgreement.Value);
            companyResult.ActivityStatus.Should().Be(company.ActivityStatus);
            companyResult.CompanyKey.Should().Be(company.CompanyKey?.ToString());
            companyResult.FirstName.Should().Be(company.FirstName);
            companyResult.LastName.Should().Be(company.LastName);
            companyResult.CellNumber.Should().Be(company.CellNumber);
            companyResult.BankNumber.Should().Be(company.BankNumber);
            companyResult.BankName.Should().Be(company.BankName);
            companyResult.BankAccountNumber.Should().Be(company.BankAccountNumber);
            companyResult.XyziesId.Should().Be(company.XyziesId);
            companyResult.ApprovedDate.Should().Be(company.ApprovedDate.Value);
            companyResult.BankInfoGiven.Should().Be(company.BankInfoGiven.Value);
            companyResult.AccountManager.Should().Be(company.AccountManager.Value);
            companyResult.CrmCompanyId.Should().Be(company.CrmCompanyId);
            companyResult.IsCallCenter.Should().Be(company.IsCallCenter.Value);
            companyResult.ParentCompanyId.Should().Be(company.ParentCompanyId);
            companyResult.TeamKey.Should().Be(company.TeamKey.Value);
            companyResult.RetailerGroupKey.Should().Be(company.RetailerGroupKey.Value);
            companyResult.SocialMediaAccount.Should().Be(company.SocialMediaAccount);
            companyResult.RetailerGoogleAccount.Should().Be(company.RetailerGoogleAccount);
            companyResult.RetailerGooglePassword.Should().Be(company.RetailerGooglePassword);
            companyResult.PaymentMode.Should().Be(company.PaymentMode);
            companyResult.CustomerDemographicId.Should().Be(company.CustomerDemographicId);
            companyResult.LocationTypeId.Should().Be(company.LocationTypeId);
            companyResult.IsOwnerPassBackground.Should().Be(company.IsOwnerPassBackground.Value);
            companyResult.IsWebsite.Should().Be(company.IsWebsite.Value);
            companyResult.IsSellsLifelineWireless.Should().Be(company.IsSellsLifelineWireless.Value);
            companyResult.NumberofStores.Should().Be(company.NumberofStores);
            companyResult.BusinessDescription.Should().Be(company.BusinessDescription);
            companyResult.WebsiteList.Should().Be(company.WebsiteList);
            companyResult.IsSpectrum.Should().Be(company.IsSpectrum.Value);
            companyResult.BusinessSource.Should().Be(company.BusinessSource);
            companyResult.GeoLat.Should().Be(company.GeoLat);
            companyResult.GeoLon.Should().Be(company.GeoLon);
            companyResult.IsMarketPlace.Should().Be(company.IsMarketPlace.Value);
            companyResult.MarketPlaceName.Should().Be(company.MarketPlaceName);
            companyResult.PhysicalName.Should().Be(company.PhysicalName);
            companyResult.MarketStrategy.Should().Be(company.MarketStrategy);
            companyResult.CompanyStatusKey.Should().Be(company.CompanyStatusKey);
            companyResult.LogoUrl.Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWithLogoWhenGetCompanyById()
        {
            // Arrange
            int usersCount = 5;
            int branchCount = 9;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .With(x => x.Id, _baseTest.DefaultCompanyId)
                                           .Create();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();
            var file = GetFormFileMoq();
            await _baseTest.AddImageInBlobStorage(file);
            CompanyModelExtended companyResult = null;

            var branches = _baseTest.Fixture.Build<Branch>()
                                           .With(x => x.CompanyId, company.Id)
                                           .CreateMany(branchCount);

            var users = _baseTest.Fixture.Build<Users>()
                                       .With(x => x.Role, _baseTest.SalesRoleId.ToString())
                                       .With(x => x.CompanyId, company.Id)
                                       .CreateMany(usersCount);

            _baseTest.CableDbContext.Branches.AddRange(branches);
            _baseTest.CableDbContext.Users.AddRange(users);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}/{company.Id}";
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            companyResult = JsonConvert.DeserializeObject<CompanyModelExtended>(responseString);
            await _baseTest.DeleteImageInBlobStorage();
            //Assert
            companyResult.CountBranch.Should().Be(branchCount);
            companyResult.CountSalesRep.Should().Be(usersCount);
            companyResult.CompanyName.Should().Be(company.CompanyName);
            companyResult.LegalName.Should().Be(company.LegalName);
            companyResult.Email.Should().Be(company.Email);
            companyResult.Phone.Should().Be(company.Phone);
            companyResult.Address.Should().Be(company.Address);
            companyResult.City.Should().Be(company.City);
            companyResult.State.Should().Be(company.State);
            companyResult.ZipCode.Should().Be(company.ZipCode);
            companyResult.StoreID.Should().Be(company.StoreID);
            companyResult.CreatedDate.Should().Be(company.CreatedDate);
            companyResult.ModifiedDate.Should().Be(company.ModifiedDate);
            companyResult.CreatedBy.Should().Be(company.CreatedBy);
            companyResult.ModifiedBy.Should().Be(company.ModifiedBy);
            companyResult.Agentid.Should().Be(company.Agentid);
            companyResult.Status.Should().Be(company.Status);
            companyResult.StoreLocationCount.Should().Be(company.StoreLocationCount);
            companyResult.PrimaryContactName.Should().Be(company.PrimaryContactName);
            companyResult.PrimaryContactTitle.Should().Be(company.PrimaryContactTitle);
            companyResult.IsEnabled.Should().Be(company.IsEnabled);
            companyResult.Fax.Should().Be(company.Fax);
            companyResult.FedId.Should().Be(company.FedId);
            companyResult.TypeOfCompany.Should().Be(company.TypeOfCompany);
            companyResult.StateEstablished.Should().Be(company.StateEstablished);
            companyResult.CompanyType.Should().Be(company.CompanyType);
            companyResult.CallerId.Should().Be(company.CallerId);
            companyResult.IsAgreement.Should().Be(company.IsAgreement.Value);
            companyResult.ActivityStatus.Should().Be(company.ActivityStatus);
            companyResult.CompanyKey.Should().Be(company.CompanyKey?.ToString());
            companyResult.FirstName.Should().Be(company.FirstName);
            companyResult.LastName.Should().Be(company.LastName);
            companyResult.CellNumber.Should().Be(company.CellNumber);
            companyResult.BankNumber.Should().Be(company.BankNumber);
            companyResult.BankName.Should().Be(company.BankName);
            companyResult.BankAccountNumber.Should().Be(company.BankAccountNumber);
            companyResult.XyziesId.Should().Be(company.XyziesId);
            companyResult.ApprovedDate.Should().Be(company.ApprovedDate.Value);
            companyResult.BankInfoGiven.Should().Be(company.BankInfoGiven.Value);
            companyResult.AccountManager.Should().Be(company.AccountManager.Value);
            companyResult.CrmCompanyId.Should().Be(company.CrmCompanyId);
            companyResult.IsCallCenter.Should().Be(company.IsCallCenter.Value);
            companyResult.ParentCompanyId.Should().Be(company.ParentCompanyId);
            companyResult.TeamKey.Should().Be(company.TeamKey.Value);
            companyResult.RetailerGroupKey.Should().Be(company.RetailerGroupKey.Value);
            companyResult.SocialMediaAccount.Should().Be(company.SocialMediaAccount);
            companyResult.RetailerGoogleAccount.Should().Be(company.RetailerGoogleAccount);
            companyResult.RetailerGooglePassword.Should().Be(company.RetailerGooglePassword);
            companyResult.PaymentMode.Should().Be(company.PaymentMode);
            companyResult.CustomerDemographicId.Should().Be(company.CustomerDemographicId);
            companyResult.LocationTypeId.Should().Be(company.LocationTypeId);
            companyResult.IsOwnerPassBackground.Should().Be(company.IsOwnerPassBackground.Value);
            companyResult.IsWebsite.Should().Be(company.IsWebsite.Value);
            companyResult.IsSellsLifelineWireless.Should().Be(company.IsSellsLifelineWireless.Value);
            companyResult.NumberofStores.Should().Be(company.NumberofStores);
            companyResult.BusinessDescription.Should().Be(company.BusinessDescription);
            companyResult.WebsiteList.Should().Be(company.WebsiteList);
            companyResult.IsSpectrum.Should().Be(company.IsSpectrum.Value);
            companyResult.BusinessSource.Should().Be(company.BusinessSource);
            companyResult.GeoLat.Should().Be(company.GeoLat);
            companyResult.GeoLon.Should().Be(company.GeoLon);
            companyResult.IsMarketPlace.Should().Be(company.IsMarketPlace.Value);
            companyResult.MarketPlaceName.Should().Be(company.MarketPlaceName);
            companyResult.PhysicalName.Should().Be(company.PhysicalName);
            companyResult.MarketStrategy.Should().Be(company.MarketStrategy);
            companyResult.CompanyStatusKey.Should().Be(company.CompanyStatusKey);
            companyResult.LogoUrl.Should().NotBeNullOrWhiteSpace();
        }
        #endregion

        #region Post Company

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenPostCompany()
        {
            // Arrange
            string uri = $"{_baseCompanyUrl}";
            var content = new StringContent(JsonConvert.SerializeObject(string.Empty), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.PostAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public async Task ShouldReturnBadRequestResultIfCompanyNameIsNullOrEmptyWhenPostCompany(string companyName)
        {
            // Arrange
            string uri = $"{_baseCompanyUrl}";
            var request = _baseTest.Fixture.Build<CreateCompanyModel>()
                                           .With(x => x.CompanyName, companyName)
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.Phone, "7643020430")
                                           .With(x => x.Fax, "7643020430")
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PostAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfEmailIsNotValidWhenPostCompany()
        {
            // Arrange
            string uri = $"{_baseCompanyUrl}";
            var request = _baseTest.Fixture.Build<CreateCompanyModel>()
                                           .With(x => x.Phone, "7643020430")
                                           .With(x => x.Fax, "7643020430")
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PostAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfPhoneIsNotValidWhenPostCompany()
        {
            // Arrange
            string uri = $"{_baseCompanyUrl}";
            var request = _baseTest.Fixture.Build<CreateCompanyModel>()
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.Fax, "7643020430")
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PostAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfFaxIsNotValidWhenPostCompany()
        {
            // Arrange
            string uri = $"{_baseCompanyUrl}";
            var request = _baseTest.Fixture.Build<CreateCompanyModel>()
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.Phone, "7643020430")
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PostAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfCompanyWithThisEmailAlreadyExistWhenPostCompany()
        {
            // Arrange
            string email = "test@email.com";
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .With(x => x.Email, email)
                                           .Create();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}";
            var request = _baseTest.Fixture.Build<CreateCompanyModel>()
                                           .With(x => x.Email, email)
                                           .With(x => x.Phone, "7643020430")
                                           .With(x => x.Fax, "7643020430")
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PostAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWhenPostCompany()
        {
            // Arrange
            string uri = $"{_baseCompanyUrl}";
            var request = _baseTest.Fixture.Build<CreateCompanyModel>()
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.Phone, "7643020430")
                                           .With(x => x.Fax, "7643020430")
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            int companyId = JsonConvert.DeserializeObject<int>(responseString);
            //Assert
            _baseTest.CableDbContext.Companies.Count().Should().Be(1);
            _baseTest.CableDbContext.Companies.First().Id.Should().Be(companyId);
        }
        #endregion

        #region Put Company

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenPutCompany()
        {
            // Arrange
            int companyId = _baseTest.Fixture.Create<int>();
            string uri = $"{_baseCompanyUrl}/{companyId}";
            var content = new StringContent(JsonConvert.SerializeObject(string.Empty), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.PutAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public async Task ShouldReturnBadRequestResultIfCompanyNameIsNullOrEmptyWhenPutCompany(string companyName)
        {
            // Arrange
            int companyId = _baseTest.Fixture.Create<int>();
            string uri = $"{_baseCompanyUrl}/{companyId}";
            var request = _baseTest.Fixture.Build<CreateCompanyModel>()
                                           .With(x => x.CompanyName, companyName)
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.Phone, "7643020430")
                                           .With(x => x.Fax, "7643020430")
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfEmailIsNotValidWhenPutCompany()
        {
            // Arrange
            int companyId = _baseTest.Fixture.Create<int>();
            string uri = $"{_baseCompanyUrl}/{companyId}";
            var request = _baseTest.Fixture.Build<CreateCompanyModel>()
                                           .With(x => x.Phone, "7643020430")
                                           .With(x => x.Fax, "7643020430")
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfPhoneIsNotValidWhenPutCompany()
        {
            // Arrange
            int companyId = _baseTest.Fixture.Create<int>();
            string uri = $"{_baseCompanyUrl}/{companyId}";
            var request = _baseTest.Fixture.Build<CreateCompanyModel>()
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.Fax, "7643020430")
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfFaxIsNotValidWhenPutCompany()
        {
            // Arrange
            int companyId = _baseTest.Fixture.Create<int>();
            string uri = $"{_baseCompanyUrl}/{companyId}";
            var request = _baseTest.Fixture.Build<CreateCompanyModel>()
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.Phone, "7643020430")
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultWhenPutCompany()
        {
            // Arrange
            int companyId = _baseTest.Fixture.Create<int>();
            string uri = $"{_baseCompanyUrl}/{companyId}";
            var request = _baseTest.Fixture.Build<CreateCompanyModel>()
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.Phone, "7643020430")
                                           .With(x => x.Fax, "7643020430")
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, content);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        //TODO Is Need this test
        //[Fact]
        //public async Task ShouldReturnBadRequestResultIfCompanyWithThisEmailAlreadyExistWhenPutCompany()
        //{
        //    // Arrange
        //    string email = "test@email.com";
        //    var requestStatusOnBoarder = _baseTest.DbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
        //    var firstCompany = _baseTest.Fixture.Build<Company>()
        //                                   .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
        //                                   .With(x => x.Email, email)
        //                                   .Create();
        //    var secondCompany = _baseTest.Fixture.Build<Company>()
        //                                   .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
        //                                   .Create();
        //    _baseTest.DbContext.Companies.Add(firstCompany);
        //    _baseTest.DbContext.Companies.Add(secondCompany);
        //    _baseTest.DbContext.SaveChanges();

        //    string uri = $"{_baseCompanyUrl}/{secondCompany.Id}";
        //    var request = _baseTest.Fixture.Build<CreateCompanyModel>()
        //                                   .With(x => x.Email, email)
        //                                   .With(x => x.Phone, "7643020430")
        //                                   .With(x => x.Fax, "7643020430")
        //                                   .Create();
        //    var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        //    // Act
        //    _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
        //    var response = await _baseTest.HttpClient.PutAsync(uri, content);

        //    //Assert
        //    response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        //}

        [Fact]
        public async Task ShouldReturnSuccessResultWhenPutCompany()
        {
            // Arrange
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .Create();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();
            string uri = $"{_baseCompanyUrl}/{company.Id}";
            var request = _baseTest.Fixture.Build<CreateCompanyModel>()
                                           .With(x => x.Email, "test@email.com")
                                           .With(x => x.Phone, "7643020430")
                                           .With(x => x.Fax, "7643020430")
                                           .Create();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, content);
            response.EnsureSuccessStatusCode();

            await _baseTest.CableDbContext.Entry(company).ReloadAsync();
            var companyFromDb = _baseTest.CableDbContext.Companies.First(x => x.Id == company.Id);
            //Assert
            companyFromDb.CompanyName.Should().Be(request.CompanyName);
            companyFromDb.LegalName.Should().Be(request.LegalName);
            companyFromDb.Email.Should().Be(request.Email);
            companyFromDb.Phone.Should().Be(request.Phone);
            companyFromDb.Address.Should().Be(request.Address);
            companyFromDb.City.Should().Be(request.City);
            companyFromDb.State.Should().Be(request.State);
            companyFromDb.ZipCode.Should().Be(request.ZipCode);
            companyFromDb.StoreID.Should().Be(request.StoreID);
            companyFromDb.Agentid.Should().Be(request.Agentid);
            companyFromDb.Status.Should().Be(request.Status);
            companyFromDb.PrimaryContactName.Should().Be(request.PrimaryContactName);
            companyFromDb.PrimaryContactTitle.Should().Be(request.PrimaryContactTitle);
            companyFromDb.Fax.Should().Be(request.Fax);
            companyFromDb.FirstName.Should().Be(request.FirstName);
            companyFromDb.GeoLat.Should().Be(request.GeoLat);
            companyFromDb.GeoLon.Should().Be(request.GeoLog);
            companyFromDb.IsEnabled.Should().Be(request.IsEnabled);
        }
        #endregion

        #region Patch company

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenPatchCompany()
        {
            // Arrange
            int companyId = _baseTest.Fixture.Create<int>();
            string uri = $"{_baseCompanyUrl}/{companyId}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.PatchAsync(uri, null);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultIfCompanyNotExistWhenPatchCompany()
        {
            // Arrange
            int companyId = _baseTest.Fixture.Create<int>();
            string uri = $"{_baseCompanyUrl}/{companyId}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PatchAsync(uri, null);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultIfCompanyExistButRequestStatusNotOnBoardedWhenPatchBranch()
        {
            // Arrange
            var company = _baseTest.Fixture.Create<Company>();
            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();
            string uri = $"{_baseCompanyUrl}/{company.Id}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PatchAsync(uri, null);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWhenPatchCompany()
        {
            // Arrange
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .With(x => x.IsEnabled, false)
                                           .Create();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();
            string uri = $"{_baseCompanyUrl}/{company.Id}?isEnabled={true}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PatchAsync(uri, null);
            response.EnsureSuccessStatusCode();

            await _baseTest.CableDbContext.Entry(company).ReloadAsync();
            var companyFromDb = _baseTest.CableDbContext.Companies.First(x => x.Id == company.Id);
            //Assert
            companyFromDb.IsEnabled.Should().BeTrue();
        }
        #endregion

        #region UpdateCompanyAvatar

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenUpdateCompanyAvatar()
        {
            // Arrange
            int companyId = _baseTest.Fixture.Create<int>();
            string uri = $"{_baseCompanyUrl}/{companyId}/avatar";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.PutAsync(uri, null);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnBadRequestResultIfNotSendFileWhenUpdateCompanyAvatar()
        {
            // Arrange
            int companyId = _baseTest.Fixture.Create<int>();
            string uri = $"{_baseCompanyUrl}/{companyId}/avatar";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, null);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Theory]
        [InlineData("test.txt")]
        [InlineData("test.jpeg")]
        public async Task ShouldReturnBadRequestResultIfFileIsNotImageOrHasBigSizeWhenUpdateCompanyAvatar(string path)
        {
            // Arrange
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .With(x => x.Id, _baseTest.DefaultCompanyId)
                                           .Create();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}/{company.Id}/avatar";
            var index = Directory.GetCurrentDirectory().ToString().IndexOf("bin");

            string productFolder = Directory.GetCurrentDirectory().ToString().Substring(0, index);
            var fileStream = File.OpenRead(Path.Combine(productFolder, "TestFiles", path));
            byte[] data;
            using (var br = new BinaryReader(fileStream))
                data = br.ReadBytes((int)fileStream.Length);

            ByteArrayContent bytes = new ByteArrayContent(data);

            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            multiContent.Add(bytes, "file", fileStream.Name);
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, multiContent);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultIfCompanyNotExistWhenUpdateCompanyAvatar()
        {
            // Arrange
            string uri = $"{_baseCompanyUrl}/{_baseTest.DefaultCompanyId}/avatar";
            var file = GetFormFileMoq();
            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream()))
                data = br.ReadBytes((int)file.OpenReadStream().Length);

            ByteArrayContent bytes = new ByteArrayContent(data);


            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            multiContent.Add(bytes, "file", file.FileName);
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, multiContent);
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory]
        [InlineData(".jpg", 102400)]
        [InlineData(".jpeg", 0)]
        [InlineData(".png", 20)]
        [InlineData(".ico", 0)]
        public async Task ShouldReturnSuccessResultWhenUpdateCompanyAvatar(string extension, long length)
        {
            // Arrange
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .With(x => x.Id, _baseTest.DefaultCompanyId)
                                           .Create();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}/{company.Id}/avatar";
            var file = GetFormFileMoq(extension, length);
            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream()))
                data = br.ReadBytes((int)file.OpenReadStream().Length);

            ByteArrayContent bytes = new ByteArrayContent(data);


            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            multiContent.Add(bytes, "file", file.FileName);
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.PutAsync(uri, multiContent);
            response.EnsureSuccessStatusCode();
            await _baseTest.DeleteImageInBlobStorage();
            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
        #endregion

        #region GetAnyCompanyAsync

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenGetAnyCompanyAsync()
        {
            // Arrange
            string token = _baseTest.Fixture.Create<string>();
            string uri = $"{_baseCompanyUrl}/{token}/trusted/internal";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnForbiddenResultWhenGetAnyCompanyAsync()
        {
            // Arrange
            string token = _baseTest.Fixture.Create<string>();
            string uri = $"{_baseCompanyUrl}/{token}/trusted/internal";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task ShouldReturnNotFoundByIdResultWhenGetAnyCompanyAsync()
        {
            // Arrange
            int companyId = 1;
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.Id, _baseTest.DefaultCompanyId)
                                           .Create();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}/{Consts.StaticToken}/trusted/internal?{nameof(CompanyMinRequestModel.Id)}={companyId}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task ShouldReturnNotFoundByCompanyNameResultWhenGetAnyCompanyAsync()
        {
            // Arrange
            string companyName = "Test";
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.Id, _baseTest.DefaultCompanyId)
                                           .Create();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}/{Consts.StaticToken}/trusted/internal?{nameof(CompanyMinRequestModel.CompanyName)}={companyName}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task ShouldReturnSuccessFoundedByIdResultWhenGetAnyCompanyAsync()
        {
            // Arrange
            var company = _baseTest.Fixture.Create<Company>();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}/{Consts.StaticToken}/trusted/internal?{nameof(CompanyMinRequestModel.Id)}={company.Id}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<CompanyMin>(responseString);
            //Assert
            result.Id.Should().Be(company.Id);
            result.CompanyName.Should().Be(company.CompanyName);
            result.CreatedDate.Should().Be(company.CreatedDate);
        }

        [Fact]
        public async Task ShouldReturnSuccessFoundedByCompanyNameResultWhenGetAnyCompanyAsync()
        {
            // Arrange
            var company = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.Id, _baseTest.DefaultCompanyId)
                                           .Create();

            _baseTest.CableDbContext.Companies.Add(company);
            _baseTest.CableDbContext.SaveChanges();

            string uri = $"{_baseCompanyUrl}/{Consts.StaticToken}/trusted/internal?{nameof(CompanyMinRequestModel.CompanyName)}={company.CompanyName}";

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<CompanyMin>(responseString);
            //Assert
            result.Id.Should().Be(company.Id);
            result.CompanyName.Should().Be(company.CompanyName);
            result.CreatedDate.Should().Be(company.CreatedDate);
        }

        #endregion

        private IFormFile GetFormFileMoq(string extension = null, long length = 0)
        {
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            string fileName;
            if (extension == null)
            {
                fileName = "test.jpeg";
            }
            else
            {
                fileName = $"test{extension}";
            }
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns(fileName);
            if (length == 0)
            {
                fileMock.Setup(x => x.Length).Returns(ms.Length);
            }
            else
            {
                fileMock.Setup(x => x.Length).Returns(length);
            }

            return fileMock.Object;
        }
    }
}
