using System.Collections.Generic;
using graphApiService.Entities.User;
using graphApiService.Services;
using tests.Services;
using Xunit;

namespace tests
{
    public class UsersControllerTest
    {
        private IUserService _userService;
        public UsersControllerTest()
        {
            _userService = new GraphClientServiceMock();
        }
        [Fact]
        public async void GetAll_WhenCalled_ReturnsCollectionOfUsers()
        {
            //Act
            var result =  await _userService.GetAllUsersAsync();

            Assert.IsType<List<ProfileDto>>(result);
        }
        [Fact]
        public async void GetById_WhenCalled_ReturnsUser_WithConcretName()
        {
            //Act
            var result = await _userService.GetUserByIdAsync("1");

            Assert.Equal("User ¹1",result.UserName);
        }
    }
}
