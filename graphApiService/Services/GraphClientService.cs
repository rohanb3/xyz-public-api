using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using graphApiService.Dtos.AzureAdGraphApi;
using graphApiService.Dtos.User;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using Microsoft.Extensions.Configuration;

namespace graphApiService.Services
{
    public interface IGraphClientService
    {
        Task<List<UserProfileDto>> GetAllUsers();
        UserProfileDto GetUserByObjectId(string objectId);
        Task<UserProfileDto> UpdateUserByObjectId(string objectId, UserProfileEditableDto userToUpdate);
        Task<UserProfileDto> CreateUserAsync(UserProfileCreatableDto user);
    }

    public class GraphClientService : IGraphClientService
    {
        private readonly ActiveDirectoryClient _client;
        private readonly IConfiguration _configuration;

        public GraphClientService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var servicePointUri = new Uri(configuration["AzureAdGraphApi:Resource"]);
            var serviceRoot = new Uri(servicePointUri, configuration["AzureAdB2C:Domain"]);
            _client = new ActiveDirectoryClient(serviceRoot, async () => await AcquireTokenAsyncForApplication());
        }

        private async Task<string> AcquireTokenAsyncForApplication()
        {
            using (var httpClient = new HttpClient())
            {
                var pairs = new List<KeyValuePair<string, string>>
                 {
                     new KeyValuePair<string, string>("client_id", _configuration["AzureAdGraphApi:client_id"]),
                     new KeyValuePair<string, string>("client_secret", _configuration["AzureAdGraphApi:client_secret"]),
                     new KeyValuePair<string, string>("Resource", _configuration["AzureAdGraphApi:Resource"]),
                     new KeyValuePair<string, string>("grant_type", _configuration["AzureAdGraphApi:grant_type"])
                 };


                FormUrlEncodedContent content = new FormUrlEncodedContent(pairs);
                HttpResponseMessage response = httpClient.PostAsync(_configuration["AzureAdGraphApi:requestUri"], content).Result;
                var credentials = await response.Content.ReadAsAsync<AzureAdGraphApiCredentials>(new[] { new JsonMediaTypeFormatter() });
                return credentials.AccessToken;
            }
        }

        public async Task<List<UserProfileDto>> GetAllUsers()
        {
            IPagedCollection<IUser> users = _client.Users.ExecuteAsync().Result;
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
            if (objectId == null) throw new ArgumentNullException(nameof(objectId));
            if (userToUpdate == null) throw new ArgumentNullException(nameof(userToUpdate));

            IUser userByObjectId = _client.Users.GetByObjectId(objectId).ExecuteAsync().Result ??
                                   throw new ObjectNotFoundException(HttpStatusCode.NotFound, "Not found");


            Utils.MergeObjects(userToUpdate, (User)userByObjectId);

            await userByObjectId.UpdateAsync();

            return userByObjectId.ToUserProfileDto();
        }

        public async Task<UserProfileDto> CreateUserAsync(UserProfileCreatableDto userToBeAdded)
        {
            if (userToBeAdded == null) throw new ArgumentNullException(nameof(userToBeAdded));

            await _client.Users.AddUserAsync(userToBeAdded.ToAdUser());

            try
            {
                return userToBeAdded.ToUserProfileDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public UserProfileDto GetUserByObjectId(string objectId)
        {
            if (objectId == null) throw new ArgumentNullException(nameof(objectId));

            User userByObjectId = (User)_client.Users.Where(user => user.ObjectId == objectId).ExecuteAsync()
                .Result.CurrentPage.First() ?? throw new ObjectNotFoundException("User not found");

            return userByObjectId.ToUserProfileDto();
        }
    }
}