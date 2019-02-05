using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using graphApiService.Dtos.AzureAdGraphApi;
using graphApiService.Dtos.User;
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
using Newtonsoft.Json.Serialization;

namespace graphApiService.Repositories.Azure
{
    public class AzureADClient : IAzureADClient
    {
        private readonly AzureAdGraphApiOptions _azureAdGraphApiOptions;
        private readonly AzureAdB2COptions _azureAdB2COptions;
        private AzureAdApiCredentials _credentials;


        public AzureADClient(IOptionsMonitor<AzureAdGraphApiOptions> azureAdGraphApiOptionsMonitor, IOptionsMonitor<AzureAdB2COptions> azureAdB2COptionsMonitor)
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

        public async Task DeleteUser(string objectId)
        {

            throw new NotImplementedException();
        }

        public async Task<UserProfileDto> GetUserById(string objectId)
        {
            using (var httpClient = new HttpClient())
            {
                await SetClient(httpClient, $"{Const.GraphApi.UserEntity}/{objectId}");
                var response = await httpClient.GetAsync(httpClient.BaseAddress);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ObjectNotFoundException("User with current identifier does not exist");
                }
                var responseString = await response.Content?.ReadAsStringAsync();
                var value = (JToken)JsonConvert.DeserializeObject(responseString);
                return value.ToObject<UserProfileDto>();
            }
        }

        public async Task<IEnumerable<UserProfileDto>> GetUsers()
        {
            using (var httpClient = new HttpClient())
            {
                await SetClient(httpClient, Const.GraphApi.UserEntity);
                var response = await httpClient.GetAsync(httpClient.BaseAddress);
                if (response.Content != null)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var value = ((JToken)JsonConvert.DeserializeObject(responseString))["value"];

                    return value.ToObject<List<UserProfileDto>>();
                }
                return null;
            }
        }

        public Task PatchUser(string objectId, UserProfileEditableDto user)
        {
            throw new NotImplementedException();
        }

        public async Task PostUser(UserProfileCreatableDto user)
        {
            using (var httpClient = new HttpClient())
            {
                await SetClient(httpClient, Const.GraphApi.UserEntity);
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var content = new StringContent(JsonConvert.SerializeObject(user, serializerSettings), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(httpClient.BaseAddress, content);
                var responseString = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
