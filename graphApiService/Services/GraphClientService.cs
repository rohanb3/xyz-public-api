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
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using Microsoft.Extensions.Options;

namespace graphApiService.Services
{
    public interface IGraphClientService
    {
        Task<List<UserProfileDto>> GetAllUsers();
        Task<UserProfileDto> GetUserByObjectId(string objectId);
        Task<UserProfileDto> UpdateUserByObjectId(string objectId, UserProfileEditableDto userToUpdate);
        Task<UserProfileDto> CreateUserAsync(UserProfileCreatableDto user);
    }

    public class GraphClientService : IGraphClientService
    {
        private readonly ActiveDirectoryClient _client;
        private readonly AzureAdGraphApiOptions _azureAdGraphApiOptions;

        public GraphClientService(IOptionsMonitor<AzureAdGraphApiOptions> azureAdGraphApiOptionsMonitor,
            IOptionsMonitor<AzureAdB2COptions> azureAdB2COptionsMonitor)
        {
            var azureAdB2COptions = azureAdB2COptionsMonitor?.CurrentValue ??
                                    throw new ArgumentNullException(nameof(azureAdB2COptionsMonitor));
            _azureAdGraphApiOptions = azureAdGraphApiOptionsMonitor?.CurrentValue ??
                                      throw new ArgumentNullException(nameof(azureAdGraphApiOptionsMonitor));

            var servicePointUri = new Uri(_azureAdGraphApiOptions.Resource);
            var serviceRoot = new Uri(servicePointUri, azureAdB2COptions.Domain);
            _client = new ActiveDirectoryClient(serviceRoot, async () => await AcquireTokenAsyncForApplication());
        }

        private async Task<string> AcquireTokenAsyncForApplication()
        {
            using (var httpClient = new HttpClient())
            {
                var pairs = new List<KeyValuePair<string, string>>
                 {
                     new KeyValuePair<string, string>("client_id", _azureAdGraphApiOptions.ClientId),
                     new KeyValuePair<string, string>("client_secret", _azureAdGraphApiOptions.ClientSecret),
                     new KeyValuePair<string, string>("Resource", _azureAdGraphApiOptions.Resource),
                     new KeyValuePair<string, string>("grant_type", _azureAdGraphApiOptions.GrantType)
                 };


                FormUrlEncodedContent content = new FormUrlEncodedContent(pairs);
                HttpResponseMessage response = await httpClient.PostAsync(_azureAdGraphApiOptions.RequestUri, content);
                var credentials = await response.Content.ReadAsAsync<AzureAdGraphApiCredentials>(new[] { new JsonMediaTypeFormatter() });
                return credentials.AccessToken;
            }
        }

        public async Task<List<UserProfileDto>> GetAllUsers()
        {
            IPagedCollection<IUser> users = await _client.Users.ExecuteAsync();
            List<UserProfileDto> result = new List<UserProfileDto>();

            do
            {
                foreach (var user in users.CurrentPage)
                {
                    result.Add(user.ToUserProfileDto());
                }

                users = await users.GetNextPageAsync();
            } while (users?.MorePagesAvailable == true);

            return result;
        }

        public async Task<UserProfileDto> UpdateUserByObjectId(string objectId, UserProfileEditableDto userToUpdate)
        {
            if (objectId == null)
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            if (userToUpdate == null)
            {
                throw new ArgumentNullException(nameof(userToUpdate));
            }

            IUser userByObjectId = await _client.Users.GetByObjectId(objectId).ExecuteAsync();
            if (userByObjectId == null)
            {
                throw new ObjectNotFoundException(HttpStatusCode.NotFound, "Not found");
            }

            Utils.MergeObjects(userToUpdate, (User)userByObjectId);

            await userByObjectId.UpdateAsync();

            return userByObjectId.ToUserProfileDto();
        }

        public async Task<UserProfileDto> CreateUserAsync(UserProfileCreatableDto userToBeAdded)
        {
            if (userToBeAdded == null)
            {
                throw new ArgumentNullException(nameof(userToBeAdded));
            }

            await _client.Users.AddUserAsync(userToBeAdded.ToAdUser());

            return userToBeAdded.ToUserProfileDto();
        }

        public async Task<UserProfileDto> GetUserByObjectId(string objectId)
        {
            if (objectId == null)
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            IPagedCollection<IUser> pagedUsersCollection = await _client.Users.Where(user => user.ObjectId == objectId).ExecuteAsync();

            if (pagedUsersCollection.CurrentPage.Count == 0)
            {
                throw new ObjectNotFoundException("User with passed objectId not found");
            }

            IUser userByObjectId = pagedUsersCollection.CurrentPage.First();
            return userByObjectId.ToUserProfileDto();
        }
    }
}