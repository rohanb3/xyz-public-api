using System;
using System.Collections.Generic;
using graphApiService.Dtos.User;
using graphApiService.Services;
using tests.Services;
using Xunit;

namespace tests
{
    public class UsersControllerTest
    {
        private IGraphClientService _graphClient;
        public UsersControllerTest()
        {
            _graphClient = new GraphClientServiceMock();
        }
        [Fact]
        public async void GetAll_WhenCalled_ReturnsCollectionOfUsers()
        {
            //Act
            var result =  await _graphClient.GetAllUsers();

            Assert.IsType<List<UserProfileDto>>(result);
        }
        [Fact]
        public async void GetById_WhenCalled_ReturnsUser_WithConcretName()
        {
            //Act
            var result = await _graphClient.GetUserByObjectId("1");

            Assert.Equal("User ¹1",result.UserName);
        }
    }
}
