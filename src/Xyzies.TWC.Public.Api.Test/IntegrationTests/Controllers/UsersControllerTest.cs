using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Xyzies.TWC.Public.Api.Tests.IntegrationTests.Controllers
{
    public class UsersControllerTest : IClassFixture<BaseTest>
    {
        private readonly BaseTest _baseTest = null;
        private readonly string _baseUserUrl = null;

        public UsersControllerTest(BaseTest baseTest)
        {
            _baseTest = baseTest ?? throw new ArgumentNullException(nameof(baseTest));
            _baseTest.DbContext.ClearContext();

            _baseUserUrl = "users";
        }

        [Fact]
        public async Task ShouldReturnNotFountResultIFUserNotExistWhenGetUserOnCallWith()
        {
            // Arrange
            int cpUserId = -1;

            // Act
            var response = await _baseTest.HttpClient.GetAsync($"{_baseUserUrl}/user-on-call/{cpUserId}");

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task ShouldReturnSuccessResultWhenGetUserOnCallWith()
        {
            // Arrange
            // Act
            var response = await _baseTest.HttpClient.GetAsync($"{_baseUserUrl}/user-on-call/10011");//{_baseTest.AdminProfile.CPUserId}
            response.EnsureSuccessStatusCode();

            //Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

    }
}
