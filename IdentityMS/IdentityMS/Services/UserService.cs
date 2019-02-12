using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using graphApiService.Common;
using graphApiService.Entities.User;
using graphApiService.Common.Azure;
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

        public async Task<IEnumerable<Profile>> GetAllUsersAsync()
        {
            var users = await _azureClient.GetUsers();
            var result = new List<Profile>();

            users.ToList().ForEach(user => result.Add(user.ToProfileDto()));

            return result;
        }

        public async Task UpdateUserByIdAsync(string id, BaseProfile model)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            try
            {
                await _azureClient.PatchUser(id, model);
            }
            catch (ApplicationException)
            {
                throw;
            }
        }

        public async Task<Profile> CreateUserAsync(ProfileCreatable model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            try
            {
                await _azureClient.PostUser(model);
                return model.ToProfileDto();
            }
            catch (ApplicationException)
            {
                throw;
            }
        }

        public async Task<Profile> GetUserByIdAsync(string id)
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