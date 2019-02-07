using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using graphApiService.Helpers.Azure;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using graphApiService.Helpers;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using graphApiService.Entities.AzureAdGraphApi;
using graphApiService.Entities.User;
using graphApiService.Services;

namespace graphApiService.Repositories.Azure
{
    public class AzureAdClient : IAzureAdClient
    {
        private readonly AzureAdGraphApiOptions _azureAdGraphApiOptions;
        private readonly AzureAdB2COptions _azureAdB2COptions;
        private AzureAdApiCredentials _credentials;


        public AzureAdClient(IOptionsMonitor<AzureAdGraphApiOptions> azureAdGraphApiOptionsMonitor, IOptionsMonitor<AzureAdB2COptions> azureAdB2COptionsMonitor)
        {
            _azureAdGraphApiOptions = azureAdGraphApiOptionsMonitor?.CurrentValue ??
                                      throw new ArgumentNullException(nameof(azureAdGraphApiOptionsMonitor));
            _azureAdB2COptions = azureAdB2COptionsMonitor?.CurrentValue ??
                                      throw new ArgumentNullException(nameof(azureAdB2COptionsMonitor));
        }

        private async Task SetCredentials(HttpClient client)
        {
            if (_credentials == null)
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
                    _credentials = await response.Content.ReadAsAsync<AzureAdApiCredentials>(new[] { new JsonMediaTypeFormatter() });
                }
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _credentials?.AccessToken);
        }

        private async Task SetClient(HttpClient client, string entity, string query = "")
        {
            var queryString = HttpUtility.ParseQueryString(query);
            queryString[Const.GraphApi.ApiVersionParameter] = Const.GraphApi.ApiVersion;
            client.BaseAddress = new Uri($"{Const.GraphApi.GraphApiEndpoint}{_azureAdB2COptions.Domain}/{entity}?{queryString}");
            await SetCredentials(client);
        }

        public async Task DeleteUser(string id)
        {
            using (var httpClient = new HttpClient())
            {
                await SetClient(httpClient, $"{Const.GraphApi.UserEntity}/{id}");
                var response = await httpClient.DeleteAsync(httpClient.BaseAddress);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ObjectNotFoundException("User with current identifier does not exist");
                }
                if(response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new AccessViolationException();
                }
            }
        }

        public async Task<UserModel> GetUserById(string id)
        {
            using (var httpClient = new HttpClient())
            {
                await SetClient(httpClient, $"{Const.GraphApi.UserEntity}/{id}");
                var response = await httpClient.GetAsync(httpClient.BaseAddress);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ObjectNotFoundException("User with current identifier does not exist");
                }
                var responseString = await response.Content?.ReadAsStringAsync();
                var value = (JToken)JsonConvert.DeserializeObject(responseString);
                return value.ToObject<UserModel>();
            }
        }

        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            using (var httpClient = new HttpClient())
            {
                await SetClient(httpClient, Const.GraphApi.UserEntity);
                var response = await httpClient.GetAsync(httpClient.BaseAddress);
                var responseString = await response?.Content?.ReadAsStringAsync();
                var value = ((JToken)JsonConvert.DeserializeObject(responseString))["value"];

                return value.ToObject<List<UserModel>>();
            }
        }

        public async Task PatchUser(string id, ProfileEditableDto user)
        {
            using (var httpClient = new HttpClient())
            {
                await SetClient(httpClient, $"{Const.GraphApi.UserEntity}/{id}");
                var content = new StringContent(JsonConvert.SerializeObject(user.ToUserModel()), Encoding.UTF8, "application/json");
                var response = await httpClient.PatchAsync(httpClient.BaseAddress, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Can not update user with current parameters");
                }
            }
        }

        public async Task PostUser(ProfileCreatableDto user)
        {
            using (var httpClient = new HttpClient())
            {
                await SetClient(httpClient, Const.GraphApi.UserEntity);
                //var serializerSettings = new JsonSerializerSettings();
                //serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var content = new StringContent(JsonConvert.SerializeObject(user/*, serializerSettings*/), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(httpClient.BaseAddress, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException("Can not create user with current parameters");
                }
            }
        }
    }
}
