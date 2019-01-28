using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using graphApiService.Dtos.User;
using graphApiService.Services;

namespace tests.Services
{
    class GraphClientServiceMock: IGraphClientService
    {
        private static readonly List<UserProfileDto> MockedUsers = new List<UserProfileDto>();

        public GraphClientServiceMock()
        {
            for (int i = 0; i < 10; i++)
            {
                MockedUsers.Add(new UserProfileDto()
                {
                    AccountEnabled = true,
                    AvatarUrl = new Url($"https://some.static.resource.com/{i}/avatar"),
                    City = $"Dnipro {i}",
                    CompanyId = i,
                    CompanyName = $"Company №{i}",
                    DisplayName = $"User №{i}",
                    Email = $"{i*i}test{i*i}@test.com",
                    GivenName = "Test User",
                    ObjectId = i.ToString(),
                    Phone = $"{Math.Pow(i,i)}",
                    UserName = $"User №{i}"
                });
            }
        }

        public async Task<List<UserProfileDto>> GetAllUsers()
        {
            return MockedUsers;
        }

        public async Task<UserProfileDto> GetUserByObjectId(string objectId)
        {
            return MockedUsers.FirstOrDefault(user => user.ObjectId == objectId);
        }

        public async Task<UserProfileDto> UpdateUserByObjectId(string objectId, UserProfileEditableDto userToUpdate)
        {
            return MockedUsers.FirstOrDefault(user => user.ObjectId == objectId);
        }

        public async Task<UserProfileDto> CreateUserAsync(UserProfileCreatableDto userToCreate)
        {
            MockedUsers.Add(userToCreate.ToUserProfileDto());
            return MockedUsers.Find(user =>
                user.UserName == userToCreate.SignInNames.Find(signInName => signInName.Type == "userName").Value);
        }
    }
}
