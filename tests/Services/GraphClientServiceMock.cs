using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using graphApiService.Entities.User;
using graphApiService.Services;

namespace tests.Services
{
    class GraphClientServiceMock: IUserService
    {
        private static readonly List<ProfileDto> MockedUsers = new List<ProfileDto>();

        public GraphClientServiceMock()
        {
            for (int i = 0; i < 10; i++)
            {
                MockedUsers.Add(new ProfileDto()
                {
                    AccountEnabled = true,
                    AvatarUrl = "https://some.static.resource.com/{i}/avatar",
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
        public async Task<ProfileDto> GetUserByObjectId(string objectId)
        {
            return MockedUsers.FirstOrDefault(user => user.ObjectId == objectId);
        }

        public async Task<IEnumerable<ProfileDto>> GetAllUsersAsync()
        {
            return MockedUsers;
        }

        public async Task<ProfileDto> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserByIdAsync(string id, ProfileEditableDto userToUpdate)
        {
            MockedUsers.FirstOrDefault(user => user.ObjectId == id);
        }

        public async Task<ProfileDto> CreateUserAsync(ProfileCreatableDto toCreate)
        {
            MockedUsers.Add(toCreate.ToProfileDto());
            return MockedUsers.Find(user =>
                user.UserName == toCreate.SignInNames.Find(signInName => signInName.Type == "userName").Value);
        }

        public async Task DeleteUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
