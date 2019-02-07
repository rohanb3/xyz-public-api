using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using graphApiService.Entities.User;
using graphApiService.Helpers.Azure;
using graphApiService.Repositories.Azure;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Extensions.Options;

namespace graphApiService.Services
{
    public class UserService : IUserService
    {
        private readonly IAzureAdClient _azureClient;

        public UserService(IOptionsMonitor<AzureAdGraphApiOptions> azureAdGraphApiOptionsMonitor,
            IOptionsMonitor<AzureAdB2COptions> azureAdB2COptionsMonitor,
            IAzureAdClient azureClient)
        {
            _azureClient = azureClient ?? throw new ArgumentNullException(nameof(azureClient));
        }

        public async Task<IEnumerable<ProfileDto>> GetAllUsersAsync()
        {
            var users = await _azureClient.GetUsers();
            var result = new List<ProfileDto>();

            foreach (var user in users)
            {
                result.Add(user.ToProfileDto());
            }

            return result;
        }

        public async Task UpdateUserByIdAsync(string id, ProfileEditableDto toUpdate)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (toUpdate == null)
            {
                throw new ArgumentNullException(nameof(toUpdate));
            }

            try
            {
                await _azureClient.PatchUser(id, toUpdate);
            }
            catch (ApplicationException)
            {
                throw;
            }
        }

        public async Task<ProfileDto> CreateUserAsync(ProfileCreatableDto toBeAdded)
        {
            if (toBeAdded == null)
            {
                throw new ArgumentNullException(nameof(toBeAdded));
            }

            try
            {
                await _azureClient.PostUser(toBeAdded);
                return toBeAdded.ToProfileDto();
            }
            catch (ApplicationException)
            {
                throw;
            }
        }

        public async Task<ProfileDto> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Object id can not be null or empty");
            }

            try
            {
                var user = await _azureClient.GetUserById(id);
                return user.ToProfileDto();
            }
            catch (ObjectNotFoundException)
            {
                throw;
            }
        }

        public async Task DeleteUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Object id can not be null or empty");
            }

            try
            {
                await _azureClient.DeleteUser(id);
            }
            catch (ObjectNotFoundException)
            {
                throw;
            }
            catch (AccessViolationException)
            {
                throw;
            }
        }
    }
}