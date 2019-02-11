using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using graphApiService.Common.Azure;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using graphApiService.Common;
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

        public async Task DeleteUser(string id)
        {
            var response = await SendRequest(HttpMethod.Delete, Const.GraphApi.UserEntity, additional: id);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ObjectNotFoundException("User with current identifier does not exist");
            }
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new AccessViolationException();
            }
        }

        public async Task<AzureUser> GetUserById(string id)
        {
            var response = await SendRequest(HttpMethod.Get, Const.GraphApi.UserEntity, additional: id);
            var responseString = await response?.Content?.ReadAsStringAsync();
            var value = ((JToken)JsonConvert.DeserializeObject(responseString))["value"];
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ObjectNotFoundException("User with current identifier does not exist");
            }

            return value.ToObject<AzureUser>();
        }

        public async Task<IEnumerable<AzureUser>> GetUsers()
        {
            var response = await SendRequest(HttpMethod.Get, Const.GraphApi.UserEntity);
            var responseString = await response?.Content?.ReadAsStringAsync();
            var value = ((JToken)JsonConvert.DeserializeObject(responseString))["value"];

            return value.ToObject<List<AzureUser>>();
        }

        public async Task PatchUser(string id, ProfileEditable user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user.ToUserModel()), Encoding.UTF8, "application/json");
            var response = await SendRequest(HttpMethod.Delete, Const.GraphApi.UserEntity, content, id);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException("Can not update user with current parameters");
            }
        }

        public async Task PostUser(ProfileCreatable user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await SendRequest(HttpMethod.Delete, Const.GraphApi.UserEntity, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException("Can not create user with current parameters");
            }
        }

        private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string entity, StringContent content = null, string additional = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                await SetClient(httpClient, $"{entity} {(string.IsNullOrEmpty(additional) ? "" : $"/{additional}")}");
                return method == HttpMethod.Get ? await httpClient.GetAsync(httpClient.BaseAddress)
                     : method == HttpMethod.Delete ? await httpClient.DeleteAsync(httpClient.BaseAddress)
                     : method == HttpMethod.Post ? await httpClient.PostAsync(httpClient.BaseAddress, content)
                     : await httpClient.PatchAsync(httpClient.BaseAddress, content);
            }
        }

        private async Task SetClient(HttpClient client, string entity, string query = "")
        {
            var queryString = HttpUtility.ParseQueryString(query);
            queryString[Const.GraphApi.ApiVersionParameter] = Const.GraphApi.ApiVersion;
            client.BaseAddress = new Uri($"{Const.GraphApi.GraphApiEndpoint}{_azureAdB2COptions.Domain}/{entity}?{queryString}");
            await SetCredentials(client);
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

    }
}