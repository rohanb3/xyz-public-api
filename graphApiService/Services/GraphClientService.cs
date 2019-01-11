using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using graphApiService.Dtos.User;
using graphApiService.Entities;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using Microsoft.Extensions.Configuration;

namespace graphApiService.Services
{
    public interface IGraphClientService
    {
        IPagedCollection<IUser> GetAllUsers();
        IUser GetUserByObjectId(string objectId);
        Task<IUser> UpdateUserByObjectId(string objectId, UserProfileEditableDto userToUpdate);
        Task DeleteUserByObjectId(string objectId);
        Task<IUser> CreateUserAsync();
        Task<IUser> CreateUserAsync(IUser user);
    }

    public class GraphClientService : IGraphClientService
    {
        private readonly ActiveDirectoryClient _client;
        private readonly IConfiguration _configuration;

        public GraphClientService(IConfiguration configuration)
        {
            Uri servicePointUri = new Uri(configuration["AzureAdGraphApi:Resource"]);
            Uri serviceRoot = new Uri(servicePointUri, configuration["AzureAdB2C:Domain"]);
            _client = new ActiveDirectoryClient(serviceRoot, async () => await AcquireTokenAsyncForApplication());
            _configuration = configuration;
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
                var credentials = await response.Content.ReadAsAsync<AzureADGraphApiCredentials>(new[] { new JsonMediaTypeFormatter() });
                return credentials.AccessToken;
            }
        }

        public IPagedCollection<IUser> GetAllUsers()
        {
            return _client.Users.ExecuteAsync().Result;
        }

        public async Task<IUser> UpdateUserByObjectId(string objectId, UserProfileEditableDto userToUpdate)
        {
            IUser userByObjectId = _client.Users.GetByObjectId(objectId).ExecuteAsync().Result;

            if (userByObjectId == null)
            {
                throw new ObjectNotFoundException(HttpStatusCode.NotFound, "Not found");
            }

            userByObjectId.DisplayName = userToUpdate.DisplayName;
            userByObjectId.CompanyName = userToUpdate.CompanyName;
            userByObjectId.City = userToUpdate.City;


            try
            {
                await userByObjectId.UpdateAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Bad request");
            }

            return userByObjectId;
        }

        public async Task DeleteUserByObjectId(string objectId)
        {
            IUser userByObjectId = await _client.Users.GetByObjectId(objectId).ExecuteAsync();

            if (userByObjectId == null)
            {
                throw new ObjectNotFoundException(HttpStatusCode.NotFound, "Not found");
            }

            await userByObjectId.DeleteAsync();
        }

        public async Task<IUser> CreateUserAsync()
        {
            IUser userToBeAdded = new User()
            {
                DisplayName = " Demo User 123",
                AccountEnabled = true,
                CreationType = "LocalAccount",
                MailNickname = "DemoUser123",
                PasswordProfile = new PasswordProfile
                {
                    Password = "secret123!",
                    ForceChangePasswordNextLogin = false
                },
                UsageLocation = "US",
                SignInNames =
                {
                    new SignInName() { Type = "userName", Value = "DemoUser123" },
                    new SignInName() { Type = "emailAddress", Value = "DemoUser123@gmail.com" }
                }
            };

            await _client.Users.AddUserAsync(userToBeAdded);
            return userToBeAdded;
        }

        public async Task<IUser> CreateUserAsync(IUser userToBeAdded)
        {
            await _client.Users.AddUserAsync(userToBeAdded);
            return userToBeAdded;
        }

        public IUser GetUserByObjectId(string objectId)
        {
            return _client.Users.Where(user => user.ObjectId == objectId).ExecuteAsync().Result.CurrentPage.First();
        }
    }
}