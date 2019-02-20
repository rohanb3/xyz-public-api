﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xyzies.SSO.Identity.Data.Helpers;
using Xyzies.SSO.Identity.Data.Entity.Azure.AzureAdGraphApi;
using Xyzies.SSO.Identity.Data.Entity.Azure;

namespace Xyzies.SSO.Identity.Data.Repository.Azure
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
            var response = await SendRequest(HttpMethod.Delete, Consts.GraphApi.UserEntity, additional: id);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException("User with current identifier does not exist");
            }
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new AccessViolationException();
            }
        }

        public async Task<AzureUser> GetUserById(string id)
        {
            var response = await SendRequest(HttpMethod.Get, Consts.GraphApi.UserEntity, additional: id);
            var responseString = await response?.Content?.ReadAsStringAsync();
            var value = ((JToken)JsonConvert.DeserializeObject(responseString))["value"];
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException("User with current identifier does not exist");
            }

            return value.ToObject<AzureUser>();
        }

        public async Task<IEnumerable<AzureUser>> GetUsers(string filter = null)
        {
            var response = await SendRequest(HttpMethod.Get, Consts.GraphApi.UserEntity, query: filter);
            var responseString = await response?.Content?.ReadAsStringAsync();
            var value = ((JToken)JsonConvert.DeserializeObject(responseString))["value"];

            return value.ToObject<List<AzureUser>>();
        }

        public async Task PatchUser(string id, AzureUser user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await SendRequest(HttpMethod.Patch, Consts.GraphApi.UserEntity, content, id);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException("Can not update user with current parameters");
            }
        }

        public async Task PostUser(AzureUser user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await SendRequest(HttpMethod.Post, Consts.GraphApi.UserEntity, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException("Can not create user with current parameters");
            }
        }

        private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string entity, StringContent content = null, string additional = null, string query = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                await SetClient(httpClient, $"{entity}{(string.IsNullOrEmpty(additional) ? "" : $"/{additional}")}", query);
                return method == HttpMethod.Get ? await httpClient.GetAsync(httpClient.BaseAddress)
                     : method == HttpMethod.Delete ? await httpClient.DeleteAsync(httpClient.BaseAddress)
                     : method == HttpMethod.Post ? await httpClient.PostAsync(httpClient.BaseAddress, content)
                     : await httpClient.PatchAsync(httpClient.BaseAddress, content);
            }
        }

        private async Task SetClient(HttpClient client, string entity, string query = "")
        {
            var queryString = HttpUtility.ParseQueryString(string.IsNullOrEmpty(query) ? "" : query);
            queryString[Consts.GraphApi.ApiVersionParameter] = Consts.GraphApi.ApiVersion;
            client.BaseAddress = new Uri($"{Consts.GraphApi.GraphApiEndpoint}{_azureAdB2COptions.Domain}/{entity}?{queryString}");
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
