using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using graphApiService.Entities;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Extensions.Configuration;

namespace graphApiService.Services
{
    public interface IGraphClientService
    {
        string GetAllUsers();
        Task CreateUserAsync();
    }

    public class GraphClientService : IGraphClientService
    {
        private ActiveDirectoryClient client;
        private readonly IConfiguration Configuration;

        private async Task<string> AcquireTokenAsyncForApplication()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var pairs = new List<KeyValuePair<string, string>> 
                 {
                     new KeyValuePair<string, string>("client_id", Configuration["AzureAdGraphApi:client_id"]),
                     new KeyValuePair<string, string>("client_secret", Configuration["AzureAdGraphApi:client_secret"]),
                     new KeyValuePair<string, string>("Resource", Configuration["AzureAdGraphApi:Resource"]),
                     new KeyValuePair<string, string>("grant_type", Configuration["AzureAdGraphApi:grant_type"])
                 };


                FormUrlEncodedContent content = new FormUrlEncodedContent(pairs);
                HttpResponseMessage response = httpClient.PostAsync(Configuration["AzureAdGraphApi:requestUri"], content).Result;
                var credentials = await response.Content.ReadAsAsync<AzureADGraphApiCredentials>(new[] { new JsonMediaTypeFormatter() });
                return credentials.AccessToken;
            }
        }

        public GraphClientService(IConfiguration configuration)
        {
            Uri servicePointUri = new Uri("https://graph.windows.net");
            Uri serviceRoot = new Uri(servicePointUri, "xyziestest.onmicrosoft.com");
            client = new ActiveDirectoryClient(serviceRoot, async () => await AcquireTokenAsyncForApplication());
            Configuration = configuration;
        }

        public string GetAllUsers()
        {
            string result = "";

            foreach (var user in client.Users.ExecuteAsync().Result.CurrentPage.ToList())
            {
                result += user.DisplayName + "\n";
            }

            return result;
        }

        public async Task CreateUserAsync()
        {
            IUser userToBeAdded = new User();
            userToBeAdded.DisplayName = " Demo User";
            userToBeAdded.AccountEnabled = true;
            userToBeAdded.CreationType = "LocalAccount";
            userToBeAdded.MailNickname = "DemoUser";
            userToBeAdded.PasswordProfile = new PasswordProfile

            {
                Password = "secret123!",
                ForceChangePasswordNextLogin = false
            };
            userToBeAdded.SignInNames.Add(new SignInName() { Type = "userName", Value = "DemoUser" });
            userToBeAdded.SignInNames.Add(new SignInName() { Type = "emailAddress", Value = "DemoUser@gmail.com" });
            userToBeAdded.UsageLocation = "US";
            await client.Users.AddUserAsync(userToBeAdded);
        }
    }
}