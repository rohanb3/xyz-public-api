using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xyzies.SSO.Identity.API.Models.User;
using Xyzies.SSO.Identity.Data.Entity;
using Xyzies.SSO.Identity.Data.Repository.Azure;

namespace Xyzies.SSO.Identity.API.Service
{
    public class UserService : IUserService
    {
        private readonly IAzureAdClient _azureClient;

        public UserService(IAzureAdClient azureClient)
        {
            _azureClient = azureClient ?? throw new ArgumentNullException(nameof(azureClient));
        }

        public async Task<IEnumerable<Profile>> GetAllUsersAsync()
        {
            var users = await _azureClient.GetUsers();
            var result = new List<Profile>();
            try
            {
                return users.Adapt<List<Profile>>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                await _azureClient.PatchUser(id, model.Adapt<AzureUser>());
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
                await _azureClient.PostUser(model.Adapt<AzureUser>());
                return model.Adapt<Profile>();
                //{
                //    AccountEnabled = model.AccountEnabled,
                //    DisplayName = model.DisplayName,
                //    City = model.City,
                //    GivenName = model.GivenName,
                //    UserName = model.SignInNames.FirstOrDefault(signInName => signInName.Type == "userName")?.Value,
                //    Email = model.SignInNames.FirstOrDefault(signInName => signInName.Type == "emailAddress")?.Value,
                //};
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
                return user.Adapt<Profile>();
            }
            catch (KeyNotFoundException)
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
            catch (KeyNotFoundException)
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
