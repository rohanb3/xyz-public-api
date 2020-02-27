using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Api.Models.Filters;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Entities.TenantEntities;

namespace Xyzies.TWC.Public.Api.Tests.IntegrationTests.Controllers
{
    public class TenantControllerTest : IClassFixture<BaseTest>
    {
        private readonly BaseTest _baseTest = null;
        private readonly string _baseTenantUrl = null;

        public TenantControllerTest(BaseTest baseTest)
        {
            _baseTest = baseTest ?? throw new ArgumentNullException(nameof(baseTest));
            _baseTest.CableDbContext.ClearContext();
            _baseTest.DbContext.ClearContext();
            _baseTest.TestSeed.Seed();
            _baseTenantUrl = "tenant";
        }

        #region get simple tenant

        [Fact]
        public async Task ShouldReturnUnauthorizedResultWhenGetSimpleTenant()
        {
            // Arrange

            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _baseTest.HttpClient.GetAsync(Path.Combine(_baseTenantUrl, Consts.PrefixForBaseUrl.TenantSimple));

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWhenGetSimpleTenantEmpty()
        {
            // Arrange
            int countTenants = 2;
            int countCompany = 100;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var tenants = _baseTest.Fixture.CreateMany<Tenant>(countTenants);
            var tenantWithCompany = tenants.OrderBy(x => x.CreatedOn).First();
            var tenantWithoutCompany = tenants.OrderBy(x => x.CreatedOn).Last();
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany);

            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();
            var tenantCompanies = companies.Select(x => _baseTest.Fixture.Build<CompanyTenant>()
                                                                        .With(c => c.CompanyId, x.Id)
                                                                        .With(c => c.TenantId, tenantWithCompany.Id)
                                                                        .Create()).ToList();
            tenantWithCompany.Companies = tenantCompanies;
            _baseTest.DbContext.Tenants.AddRange(tenants);
            _baseTest.DbContext.CompanyTenants.AddRange(tenantCompanies);
            _baseTest.DbContext.SaveChanges();
            string uri = $"{Path.Combine(_baseTenantUrl, Consts.PrefixForBaseUrl.TenantSimple)}?{nameof(TenantFilterModel.TenantIds)}={Guid.NewGuid()}";
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<TenantWithCompaniesSimpleModel>>(responseString);
            int countCompanyInDb = _baseTest.CableDbContext.Companies.Count();
            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWhenGetSimpleTenantAll()
        {
            // Arrange
            int countTenants = 2;
            int countCompany = 100;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var tenants = _baseTest.Fixture.CreateMany<Tenant>(countTenants);
            var tenantWithCompany = tenants.OrderBy(x => x.CreatedOn).First();
            var tenantWithoutCompany = tenants.OrderBy(x => x.CreatedOn).Last();
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany);

            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();
            var tenantCompanies = companies.Select(x => _baseTest.Fixture.Build<CompanyTenant>()
                                                                        .With(c => c.CompanyId, x.Id)
                                                                        .With(c => c.TenantId, tenantWithCompany.Id)
                                                                        .Create()).ToList();
            tenantWithCompany.Companies = tenantCompanies;
            _baseTest.DbContext.Tenants.AddRange(tenants);
            _baseTest.DbContext.CompanyTenants.AddRange(tenantCompanies);
            _baseTest.DbContext.SaveChanges();
            string uri = Path.Combine(_baseTenantUrl, Consts.PrefixForBaseUrl.TenantSimple);
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<TenantWithCompaniesSimpleModel>>(responseString);
            int countCompanyInDb = _baseTest.CableDbContext.Companies.Count();
            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(countTenants);
            var expectedTenantWithCompany = result.First(x => x.Id == tenantWithCompany.Id);
            expectedTenantWithCompany.Name.Should().Be(tenantWithCompany.TenantName);
            expectedTenantWithCompany.Phone.Should().Be(tenantWithCompany.Phone);
            expectedTenantWithCompany.Companies.Count().Should().BeGreaterThan(0);
            expectedTenantWithCompany.Companies.Count().Should().Be(countCompany);

            var expectedTenantWithoutCompany = result.First(x => x.Id == tenantWithoutCompany.Id);
            expectedTenantWithoutCompany.Name.Should().Be(tenantWithoutCompany.TenantName);
            expectedTenantWithoutCompany.Phone.Should().Be(tenantWithoutCompany.Phone);
            expectedTenantWithoutCompany.Companies.Count().Should().Be(0);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWhenGetSimpleTenantOne()
        {
            // Arrange
            int countTenants = 2;
            int countCompany = 100;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var tenants = _baseTest.Fixture.CreateMany<Tenant>(countTenants);
            var tenantWithCompany = tenants.OrderBy(x => x.CreatedOn).First();
            var tenantWithoutCompany = tenants.OrderBy(x => x.CreatedOn).Last();
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany);

            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();
            var tenantCompanies = companies.Select(x => _baseTest.Fixture.Build<CompanyTenant>()
                                                                        .With(c => c.CompanyId, x.Id)
                                                                        .With(c => c.TenantId, tenantWithCompany.Id)
                                                                        .Create()).ToList();
            tenantWithCompany.Companies = tenantCompanies;
            _baseTest.DbContext.Tenants.AddRange(tenants);
            _baseTest.DbContext.CompanyTenants.AddRange(tenantCompanies);
            _baseTest.DbContext.SaveChanges();
            string uri = $"{Path.Combine(_baseTenantUrl, Consts.PrefixForBaseUrl.TenantSimple)}?{nameof(TenantFilterModel.TenantIds)}={tenantWithCompany.Id}";
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<TenantWithCompaniesSimpleModel>>(responseString);
            int countCompanyInDb = _baseTest.CableDbContext.Companies.Count();
            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(1);
            var expectedTenantWithCompany = result.First();
            expectedTenantWithCompany.Id.Should().Be(tenantWithCompany.Id);
            expectedTenantWithCompany.Name.Should().Be(tenantWithCompany.TenantName);
            expectedTenantWithCompany.Phone.Should().Be(tenantWithCompany.Phone);
            expectedTenantWithCompany.Companies.Count().Should().BeGreaterThan(0);
            expectedTenantWithCompany.Companies.Count().Should().Be(countCompany);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWhenGetSimpleTenantFew()
        {
            // Arrange
            int countTenants = 2;
            int countCompany = 100;
            var requestStatusOnBoarder = _baseTest.CableDbContext.RequestStatuses.First(x => x.Name == Data.Consts.OnBoardedStatusName);
            var tenants = _baseTest.Fixture.CreateMany<Tenant>(countTenants);
            var tenantWithCompany = tenants.OrderBy(x => x.CreatedOn).First();
            var tenantWithoutCompany = tenants.OrderBy(x => x.CreatedOn).Last();
            var companies = _baseTest.Fixture.Build<Company>()
                                           .With(x => x.CompanyStatusKey, requestStatusOnBoarder.Id)
                                           .CreateMany(countCompany);

            _baseTest.CableDbContext.Companies.AddRange(companies);
            _baseTest.CableDbContext.SaveChanges();
            var tenantCompanies = companies.Select(x => _baseTest.Fixture.Build<CompanyTenant>()
                                                                        .With(c => c.CompanyId, x.Id)
                                                                        .With(c => c.TenantId, tenantWithCompany.Id)
                                                                        .Create()).ToList();
            tenantWithCompany.Companies = tenantCompanies;
            _baseTest.DbContext.Tenants.AddRange(tenants);
            _baseTest.DbContext.CompanyTenants.AddRange(tenantCompanies);
            _baseTest.DbContext.SaveChanges();
            string uri = $"{Path.Combine(_baseTenantUrl, Consts.PrefixForBaseUrl.TenantSimple)}?{nameof(TenantFilterModel.TenantIds)}={tenantWithCompany.Id}&{nameof(TenantFilterModel.TenantIds)}={tenantWithoutCompany.Id}";
            // Act
            _baseTest.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_baseTest.AdminToken.TokenType, _baseTest.AdminToken.AccessToken);
            var response = await _baseTest.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<TenantWithCompaniesSimpleModel>>(responseString);
            int countCompanyInDb = _baseTest.CableDbContext.Companies.Count();
            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(countTenants);
            var expectedTenantWithCompany = result.First(x => x.Id == tenantWithCompany.Id);
            expectedTenantWithCompany.Name.Should().Be(tenantWithCompany.TenantName);
            expectedTenantWithCompany.Phone.Should().Be(tenantWithCompany.Phone);
            expectedTenantWithCompany.Companies.Count().Should().BeGreaterThan(0);
            expectedTenantWithCompany.Companies.Count().Should().Be(countCompany);

            var expectedTenantWithoutCompany = result.First(x => x.Id == tenantWithoutCompany.Id);
            expectedTenantWithoutCompany.Name.Should().Be(tenantWithoutCompany.TenantName);
            expectedTenantWithoutCompany.Phone.Should().Be(tenantWithoutCompany.Phone);
            expectedTenantWithoutCompany.Companies.Count().Should().Be(0);
        }
        #endregion
    }
}
