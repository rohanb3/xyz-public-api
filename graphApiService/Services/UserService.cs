using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using graphApiService.Dtos.AzureAdGraphApi;
using graphApiService.Dtos.User;
using graphApiService.Helpers;
using graphApiService.Helpers.Azure;
using graphApiService.Repositories.Azure;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using Microsoft.Extensions.Options;

namespace graphApiService.Services
{
    public class UserService : IUserService
    {
        private readonly IAzureADClient _azureClient;

        public UserService(IOptionsMonitor<AzureAdGraphApiOptions> azureAdGraphApiOptionsMonitor,
            IOptionsMonitor<AzureAdB2COptions> azureAdB2COptionsMonitor,
            IAzureADClient azureClient)
        {
            _azureClient = azureClient ?? throw new ArgumentNullException(nameof(azureClient));
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllUsersAsync()
        {
            var result = await _azureClient.GetUsers();
            return result;
        }

        public async Task UpdateUserByIdAsync(string id, UserProfileEditableDto userToUpdate)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (userToUpdate == null)
            {
                throw new ArgumentNullException(nameof(userToUpdate));
            }

            try
            {
                await _azureClient.PatchUser(id, userToUpdate);
            }
            catch (ApplicationException)
            {
                throw;
            }
        }

        public async Task<UserProfileDto> CreateUserAsync(UserProfileCreatableDto userToBeAdded)
        {
            if (userToBeAdded == null)
            {
                throw new ArgumentNullException(nameof(userToBeAdded));
            }

            try
            {
                await _azureClient.PostUser(userToBeAdded);
                return userToBeAdded.ToUserProfileDto();
            }
            catch (ApplicationException)
            {
                throw;
            }
        }

        public async Task<UserProfileDto> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Object id can not be null or empty");
            }

            try
            {
                return await _azureClient.GetUserById(id);
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