using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public GraphClientService(IConfiguration configuration, IMapper mapper)
        {
            Uri servicePointUri = new Uri(configuration["AzureAdGraphApi:Resource"]);
            Uri serviceRoot = new Uri(servicePointUri, configuration["AzureAdB2C:Domain"]);
            _client = new ActiveDirectoryClient(serviceRoot, async () => await AcquireTokenAsyncForApplication());
            _configuration = configuration;
            _mapper = mapper;
        }

        private async Task<string> AcquireTokenAsyncForApplication()
        {
            using (HttpClient httpClient = new HttpClient())
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
            IPagedCollection<IUser> users= _client.Users.ExecuteAsync().Result;
            List<UserProfileDto> result = new List<UserProfileDto>();

            do
            {
                foreach (var user in users.CurrentPage)
                {
                    result.Add(_mapper.Map<UserProfileDto>(user));
                }

                users = await users.GetNextPageAsync();
            } while (users?.MorePagesAvailable == true);

            return result;
        }

        public async Task<UserProfileDto> UpdateUserByObjectId(string objectId, UserProfileEditableDto userToUpdate)
        {
            IUser userByObjectId = _client.Users.GetByObjectId(objectId).ExecuteAsync().Result;

            if (userByObjectId == null)
            {
                throw new ObjectNotFoundException(HttpStatusCode.NotFound, "Not found");
            }

            Utils.MergeObjects(userToUpdate, (User)userByObjectId);

            await userByObjectId.UpdateAsync();

            return _mapper.Map<UserProfileDto>(userByObjectId);
        }

        public async Task<UserProfileDto> CreateUserAsync(UserProfileCreatableDto userToBeAdded)
        {
             await _client.Users.AddUserAsync(_mapper.Map<User>(userToBeAdded));
             return _mapper.Map<UserProfileDto>(userToBeAdded);
        }

        public UserProfileDto GetUserByObjectId(string objectId)
        {
            return _mapper.Map<UserProfileDto>(_client.Users.Where(user => user.ObjectId == objectId).ExecuteAsync().Result.CurrentPage.First());
        }
    }
}